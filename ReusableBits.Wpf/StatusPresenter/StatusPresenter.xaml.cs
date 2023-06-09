﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace ReusableBits.Wpf.StatusPresenter {
	internal enum PresenterStates {
		Active,
		Inactive
	}

	/// <summary>
	/// Interaction logic for StatusPresenter.xaml
	/// </summary>
	public partial class StatusPresenter {
		private readonly Queue<StatusMessage>	mStatusMessages;
		private readonly Storyboard ?			mPendingToActive;
		private readonly Storyboard ?			mPendingToExtendedActive;
		private readonly Storyboard ?			mActiveToExpired;
		private readonly Storyboard ?			mActiveToPending;
		private readonly DataTemplate ?		    mDefaultTemplate;
 
		public StatusPresenter() {
			mStatusMessages = new Queue<StatusMessage>();

			InitializeComponent();

			mPendingToActive = FindResource( "PendingToActive" ) as Storyboard;
			if( mPendingToActive != null ) {
				mPendingToActive.Completed += OnPendingToActiveCompleted;
			}
			mPendingToExtendedActive = FindResource( "PendingToExtendedActive" ) as Storyboard;
			if( mPendingToExtendedActive != null ) {
				mPendingToExtendedActive.Completed += OnPendingToActiveCompleted;
			}

			mActiveToExpired = FindResource( "ActiveToExpired" ) as Storyboard;
			if( mActiveToExpired != null ) {
				mActiveToExpired.Completed += OnActiveToExpiredCompleted;
			}

			mActiveToPending = FindResource( "ActiveToPending" ) as Storyboard;
			if( mActiveToPending != null ) {
				mActiveToPending.Completed += OnActiveToPendingCompleted;
			}

			mDefaultTemplate = FindResource( "DefaultTemplate" ) as DataTemplate;

			_presenter1.Tag = PresenterStates.Inactive;
			_presenter2.Tag = PresenterStates.Inactive;
		}

		public static readonly DependencyProperty StatusMessagesProperty =
			DependencyProperty.Register( nameof( StatusMessage ), typeof( StatusMessage ), typeof( StatusPresenter ),
											new FrameworkPropertyMetadata( null, FrameworkPropertyMetadataOptions.AffectsRender, OnStatusMessagesChanged ));
		public StatusMessage ? StatusMessage {
			get => (StatusMessage)GetValue( StatusMessagesProperty );
            set => SetValue( StatusMessagesProperty, value );
        }

		private static void OnStatusMessagesChanged( DependencyObject depObj, DependencyPropertyChangedEventArgs args ) {
			if(( depObj is StatusPresenter presenter ) &&
			   ( args.NewValue is StatusMessage )) {
				presenter.OnStatusMessageChanged();
			}
		}

		private void OnStatusMessageChanged() {
			if( StatusMessage != null ) {
				mStatusMessages.Enqueue( StatusMessage );

				TryToDisplayStatus();
			}
		}

		public static readonly DependencyProperty StatusMessageTemplateProperty =
			DependencyProperty.Register( nameof( StatusTemplate ), typeof( DataTemplate ), typeof( StatusPresenter),
											new FrameworkPropertyMetadata( null, FrameworkPropertyMetadataOptions.AffectsArrange ));

		public DataTemplate ? StatusTemplate {
			get => (DataTemplate)GetValue( StatusMessageTemplateProperty );
            set => SetValue( StatusMessageTemplateProperty, value );
        }

		private void TryToDisplayStatus() {
			if(!AreAnyPresentersActive()) {
				StartNextStatus();
			}
		}

		private void StartNextStatus() {
			var presenter = GetInactivePresenter();

			if(( presenter != null ) &&
			   ( mStatusMessages.Any())) {
				var status = mStatusMessages.Dequeue();

				presenter.DataContext = status;
				presenter.Tag = PresenterStates.Active;
				presenter.ContentTemplate = SelectPresenterTemplate( status.TemplateName );

				StartStatusDisplay( presenter, status.ExtendActiveDisplay ? mPendingToExtendedActive : mPendingToActive );
			}
		}

		private void StartStatusDisplay( FrameworkElement presenter, Storyboard ? storyboard ) {
			if( storyboard != null ) {
                foreach( var child in storyboard.Children ) {
                    Storyboard.SetTargetName( child, presenter.Name );
                }

                storyboard.Begin();
            }
		}

		private DataTemplate ? SelectPresenterTemplate( string templateName ) {
			var retValue = SelectDefaultTemplate();

			if(!string.IsNullOrWhiteSpace( templateName )) {
                if( TryFindResource( templateName ) is DataTemplate loadedTemplate ) {
					retValue = loadedTemplate;
				}
			}

			return( retValue );
		}

		private DataTemplate ? SelectDefaultTemplate() {
			var retValue = mDefaultTemplate;

			if( StatusTemplate != null ) {
				retValue = StatusTemplate;
			}

			return retValue ;
		}

		private ContentControl ? GetInactivePresenter() {
			var retValue = default( ContentControl );

			if( (PresenterStates)_presenter1.Tag == PresenterStates.Inactive ) {
				retValue = _presenter1;
			}
			else if( (PresenterStates)_presenter2.Tag == PresenterStates.Inactive ) {
				retValue = _presenter2;
			}

			return( retValue );
		}

		private bool AreAnyPresentersActive() {
			bool retValue = ((PresenterStates)_presenter1.Tag == PresenterStates.Active ) ||
			                ((PresenterStates)_presenter2.Tag == PresenterStates.Active );

			return ( retValue );
		}

		private void OnPendingToActiveCompleted( object ? sender, EventArgs args ) {
			if( sender is ClockGroup clockGroup ) {
                if( mStatusMessages.Count > 0 ) {
                    if( mActiveToExpired != null ) {
                        var presenter = GetTargetPresenter( clockGroup );

                        if( presenter != null ) {
                            StartStatusDisplay( presenter, mActiveToExpired );
                        }
                    }

                    StartNextStatus();
                }
                else {
                    if( mActiveToPending != null ) {
                        var presenter = GetTargetPresenter( clockGroup );

                        if( presenter != null ) {
                            StartStatusDisplay( presenter, mActiveToPending );
                        }
                    }
                }
            }
		}

		private void OnActiveToPendingCompleted( object ? sender, EventArgs args ) {
			if( sender is ClockGroup clockGroup ) {
                SetAnimationTargetToInactive( clockGroup );

                TryToDisplayStatus();
            }
		}

		private void OnActiveToExpiredCompleted( object ? sender, EventArgs args ) {
			if( sender is ClockGroup clockGroup ) {
                SetAnimationTargetToInactive( clockGroup );

                TryToDisplayStatus();
            }
		}

		private void SetAnimationTargetToInactive( ClockGroup clockGroup ) {
			var presenter = GetTargetPresenter( clockGroup );

			if( presenter != null ) {
				presenter.Tag = PresenterStates.Inactive;
			}
		}

		private FrameworkElement ? GetTargetPresenter( ClockGroup clockGroup ) {
			var	retValue = default( FrameworkElement );

			var animationClock = clockGroup.Timeline.Children.FirstOrDefault();

			if( animationClock != null ) {
				var targetName = Storyboard.GetTargetName( animationClock );

				retValue = GetPresenterByName( targetName );
			}

			return retValue;
		}

		private FrameworkElement ? GetPresenterByName( string name ) {
			var retValue = default( FrameworkElement );

			if( _presenter1.Name.Equals( name ) ) {
				retValue = _presenter1;
			}
			else if( _presenter2.Name.Equals( name ) ) {
				retValue = _presenter2;
			}

			return retValue;
		}
	}
}
