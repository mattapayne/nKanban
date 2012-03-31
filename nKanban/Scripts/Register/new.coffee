jQuery ->
    provinceLookupUrl = jQuery("#ProvinceLookupUrl").val()

    clearDDL = (ddl) -> jQuery(ddl).find("option").remove()

    addLoadingIndicator = (ddl) ->
        opt = jQuery(document.createElement("option")).val("").text("Loading ...")
        jQuery(ddl).append(opt);
    
    createSelectOption = (value, text) ->
        opt = jQuery(document.createElement("option")).val(value).text(text)
        return opt

    jQuery("#CountryId").change ->
        provinceDDL = jQuery("#ProvinceId");
        clearDDL(provinceDDL)
        
        countryId = jQuery(this).val()
        
        if(countryId)
            addLoadingIndicator(provinceDDL)
            jQuery.getJSON(provinceLookupUrl + "/" + countryId, null, (provinces) ->
                clearDDL(provinceDDL)
                provinceDDL.append(createSelectOption("", ""));

                $.each(provinces, (index, item) ->
                    provinceDDL.append(createSelectOption(item.Id, item.Name))
                )
            )