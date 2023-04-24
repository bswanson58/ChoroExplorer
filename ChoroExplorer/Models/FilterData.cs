using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace ChoroExplorer.Models {
    internal class FilterValue {
        public  string      RegionId { get; }
        public  bool        Enabled { get; }

        public FilterValue() {
            RegionId = String.Empty;
            Enabled = true;
        }

        [JsonConstructor]
        public FilterValue( string regionId, bool enabled ) {
            RegionId = regionId;
            Enabled = enabled;
        }
    }

    internal class FilterParameters {
        public  string      FilterId { get; }
        public  bool        Enabled { get; }

        public FilterParameters( FilterData forData ) {
            FilterId = forData.FilterId;
            Enabled = false;
        }

        [JsonConstructor]
        public FilterParameters( string filterId, bool enabled ) {
            FilterId = filterId;
            Enabled = enabled;
        }

        public FilterParameters With( bool enabledState ) =>
            new FilterParameters( FilterId, enabledState );
    }

    [DebuggerDisplay("FilterData:{FilterKey}")]
    internal class FilterData {
        public  string                  FilterId { get; }
        public  string                  FilterKey { get; }
        public  string                  FilterName { get; }
        public  string                  Description { get; }
        public  string                  Source { get; }
        public  List<FilterValue>       RegionFilters { get; }

        public FilterData() {
            FilterId = NCuid.Cuid.Generate();
            FilterKey = String.Empty;
            FilterName = String.Empty;
            Description = String.Empty;
            Source = String.Empty;
            RegionFilters = new List<FilterValue>();
        }

        [JsonConstructor]
        public FilterData( string filterId, string filterKey, string filterName, string description, string source, 
                           List<FilterValue> regionFilters ) {
            FilterId = filterId;
            FilterKey = filterKey;
            FilterName = filterName;
            Description = description;
            Source = source;
            RegionFilters = regionFilters;
        }
    }
}
