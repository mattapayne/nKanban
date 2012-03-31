jQuery ->
    jQuery('.dropdown-toggle').dropdown()
    jQuery("#logout-link").click((e) ->
        e.preventDefault()
        jQuery("#logout-form").submit())