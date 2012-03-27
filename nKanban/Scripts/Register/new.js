$(function () {
    var provinceLookupUrl = $("#ProvinceLookupUrl").val();

    var clearDDL = function (ddl) {
        $(ddl).find("option").remove();
    };

    var addLoadingIndicator = function (ddl) {
        var opt = $(document.createElement("option")).val("").text("Loading ...");
        $(ddl).append(opt);
    };

    var createSelectOption = function (value, text) {
        return $(document.createElement("option")).val(value).text(text);
    };

    $("#CountryId").change(function () {

        var provinceDDL = $("#ProvinceId");

        clearDDL(provinceDDL);

        var countryId = $(this).val();

        if (countryId) {

            addLoadingIndicator(provinceDDL);

            $.getJSON(provinceLookupUrl + "/" + countryId, null, function (provinces) {

                clearDDL(provinceDDL);

                provinceDDL.append(createSelectOption("", ""));

                $.each(provinces, function (index, item) {
                    provinceDDL.append(createSelectOption(item.Id, item.Name));
                });
            });
        }
    });
});