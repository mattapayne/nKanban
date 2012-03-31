(function() {

  jQuery(function() {
    jQuery('.dropdown-toggle').dropdown();
    return jQuery("#logout-link").click(function(e) {
      e.preventDefault();
      return jQuery("#logout-form").submit();
    });
  });

}).call(this);
