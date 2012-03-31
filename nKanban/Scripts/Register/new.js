(function() {

  jQuery(function() {
    var addLoadingIndicator, clearDDL, createSelectOption, provinceLookupUrl;
    provinceLookupUrl = jQuery("#ProvinceLookupUrl").val();
    clearDDL = function(ddl) {
      return jQuery(ddl).find("option").remove();
    };
    addLoadingIndicator = function(ddl) {
      var opt;
      opt = jQuery(document.createElement("option")).val("").text("Loading ...");
      return jQuery(ddl).append(opt);
    };
    createSelectOption = function(value, text) {
      var opt;
      opt = jQuery(document.createElement("option")).val(value).text(text);
      return opt;
    };
    return jQuery("#CountryId").change(function() {
      var countryId, provinceDDL;
      provinceDDL = jQuery("#ProvinceId");
      clearDDL(provinceDDL);
      countryId = jQuery(this).val();
      if (countryId) {
        addLoadingIndicator(provinceDDL);
        return jQuery.getJSON(provinceLookupUrl + "/" + countryId, null, function(provinces) {
          clearDDL(provinceDDL);
          provinceDDL.append(createSelectOption("", ""));
          return $.each(provinces, function(index, item) {
            return provinceDDL.append(createSelectOption(item.Id, item.Name));
          });
        });
      }
    });
  });

}).call(this);
