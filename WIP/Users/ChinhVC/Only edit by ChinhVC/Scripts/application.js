// Some general UI pack related JS
// Extend JS String with repeat method
String.prototype.repeat = function (num) {
  return new Array(Math.round(num) + 1).join(this);
};

(function ($) {
  //Backgourd Min-Height
  // -------------------------------------------------------------
  $(document).ready(function(){
      function bg_minHeight(){
        var bg_minHeight = $(".bg-min-height");
        var height = $(window).height();
        if (height > 0) {
            bg_minHeight.css({
              'min-height':height + 'px'
            });
        }
      }
      $(window).bind("load", function () {
         bg_minHeight();
      });
      $(window).resize(function() {
         bg_minHeight();
      });
  });
  // end Backgourd Min-Height --------------------------------------------


  //Sticky Footer
  // -------------------------------------------------------------
  $(document).ready(function(){
      function stickyFooter(){
        var footer = $("#footer");
        var bg_minHeight = $(".bg-min-height");
        var pos = footer.position();
        var height = $(window).height();
        if(pos==null){return;}
        height = height - pos.top;
        height = height - footer.height();
        if (height > 0) {
            footer.css({
                'margin-top': height + 'px'
            });
            bg_minHeight.css({
              'min-height': $(window).height() + 'px'
            });
        }

        
      }
      $(window).bind("load", function () {
         stickyFooter();
      });
      $(window).resize(function() {
         stickyFooter();
      });
  });
  // end Sticky Footer --------------------------------------------



  //#main-slider
  // -------------------------------------------------------------
  $(function(){
    $('#main-slider.carousel').carousel({
      interval: 8000
    });
  });

  $( '.centered' ).each(function( e ) {
    $(this).css('margin-top',  ($('#main-slider').height() - $(this).height())/2);
  });

  $(window).resize(function(){
    $( '.centered' ).each(function( e ) {
      $(this).css('margin-top',  ($('#main-slider').height() - $(this).height())/2);
    });
  });
  //end Main Slider ---------------------------------------------


  //#fit col .portfolio-items
  // -------------------------------------------------------------
    function fitCol_porfolio(){
      var win_width = $(window).width();
       if(win_width > 1200){
         $('.portfolio-items').removeClass('col-4');
         $('.portfolio-items').removeClass('col-3');
         $('.portfolio-items').removeClass('col-2');
         $('.portfolio-items').removeClass('col-1');
         $('.portfolio-items').addClass('col-4');
       } else if(win_width >975 && win_width <=1200){
         $('.portfolio-items').removeClass('col-4');
         $('.portfolio-items').removeClass('col-3');
         $('.portfolio-items').removeClass('col-3');
         $('.portfolio-items').removeClass('col-2');
         $('.portfolio-items').removeClass('col-1');
         $('.portfolio-items').addClass('col-3');
       } else if(win_width <=975){
         $('.portfolio-items').removeClass('col-4');
         $('.portfolio-items').removeClass('col-3');
         $('.portfolio-items').removeClass('col-3');
         $('.portfolio-items').removeClass('col-2');
         $('.portfolio-items').removeClass('col-1');
         $('.portfolio-items').addClass('col-2');
       }
    }
    $(window).bind("load", function () {
         fitCol_porfolio();
      });
    $(window).resize(function() {
       fitCol_porfolio();
    });
  //end col .portfolio-items ---------------------------------------



  //#item-image
  //------------------------------------------------------------
    $( '.img-fit' ).each(function(e) {
        var img_width = $('.item-image').width();
        var img_height = img_width*3/4;
        // var div_height = img_width*3/4; + 20;
        // var div_width = img_width + 20;
        // console.log(img_width + "x" + img_height);
        $('.img-fit').css({'width': img_width,'height': img_height});
    });

    $(window).resize(function(e){
      $( '.img-fit' ).each(function() {
        var img_width = $('.item-image').width();
        var img_height = img_width*3/4;
        // var div_height = img_width*3/4; + 20;
        // var div_width = img_width + 20;
        // console.log(img_width + "x" + img_height);
        $('.img-fit').css({'width': img_width,'height': img_height});
      });
    });
  //end item-image ---------------------------------------------


  //Scroll to top
  // -------------------------------------------------------------
  $(document).on( 'scroll', function(){
    if ($(window).scrollTop() > 100) {
      $('.scroll-top-wrapper').addClass('show');
    } else {
      $('.scroll-top-wrapper').removeClass('show');
    }
    if ($(window).scrollTop() < $(document).height()-$(window).height()-100) {
      $('.scroll-bottom-wrapper').addClass('show');
    } else {
      $('.scroll-bottom-wrapper').removeClass('show');
    }
  });
  function scrollToTop() {
    verticalOffset = typeof(verticalOffset) != 'undefined' ? verticalOffset : 0;
    element = $('body');
    offset = element.offset();
    offsetTop = offset.top;
    $('html, body').animate({scrollTop: offsetTop}, 500, 'linear');
  }

  function scrollToBottom() {
    verticalOffset = typeof(verticalOffset) != 'undefined' ? verticalOffset : 0;
    element = $('body');
    offset = element.offset();
    offsetTop = offset.top;
    $("html, body").animate({ scrollTop: $(document).height()-$(window).height() },500,'linear');
    // $('html, body').animate({scrollTop: offsetTop}, 500, 'linear');
  }
         
  $('.gototop').on('click', scrollToTop);
  $('.scroll-top-wrapper').on('click', scrollToTop);
  $('.scroll-bottom-wrapper').on('click', scrollToBottom);
  //end Scroll to top ---------------------------------------------



  //Fixed position scroll 
  // -------------------------------------------------------------
  //edit_project
  $(window).scroll(function(){
    if($('#create-tabs').length > 0){
      if ($(this).scrollTop() > $('#create-tabs-fixedPosition').offset().top){
        $('.navbar').removeClass('navbar-fixed-top');
        $('#create-tabs').addClass('fixed-Scroll');
      }
      else{
        $('.navbar').addClass('navbar-fixed-top');
        $('#create-tabs').removeClass('fixed-Scroll');
      }
    }

    //Project detail
    if($('#project_detail-tab').length > 0){
      if($(this).scrollTop() > $('#project_detail-fixedPosition').offset().top){
        $('.navbar').removeClass('navbar-fixed-top');
        $('#project_detail-tab').addClass('fixed-Scroll');
      }else{
        $('.navbar').addClass('navbar-fixed-top');
        $('#project_detail-tab').removeClass('fixed-Scroll');
      }
    } 

  });
  //end fixed position scroll edit_project ------------------------



  //Funtions Flat UI
  //---------------------------------------------------------------
  $(function () {

    // Todo list
    $('.todo').on('click', 'li', function () {
      $(this).toggleClass('todo-done');
    });

    // Custom Selects
    if ($('[data-toggle="select"]').length) {
      $('[data-toggle="select"]').select2();
    }

    // Checkboxes and Radio buttons
    $('[data-toggle="checkbox"]').radiocheck();
    $('[data-toggle="radio"]').radiocheck();

    $(':checkbox').radiocheck();
    
    // Tooltips
    $('[data-toggle=tooltip]').tooltip('show');

    // Focus state for append/prepend inputs
    $('.input-group').on('focus', '.form-control', function () {
      $(this).closest('.input-group, .form-group').addClass('focus');
    }).on('blur', '.form-control', function () {
      $(this).closest('.input-group, .form-group').removeClass('focus');
    });

    // Make pagination demo work
    $('.pagination').on('click', 'a', function () {
      $(this).parent().siblings('li').removeClass('active').end().addClass('active');
    });

    $('.btn-group').on('click', 'a', function () {
      $(this).siblings().removeClass('active').end().addClass('active');
    });

    // Disable link clicks to prevent page scrolling
    $(document).on('click', 'a[href="#fakelink"]', function (e) {
      e.preventDefault();
    });

    // Switches
    if ($('[data-toggle="switch"]').length) {
      $('[data-toggle="switch"]').bootstrapSwitch();
    }

    // Typeahead
    if ($('#typeahead-demo-01').length) {
      var states = new Bloodhound({
        datumTokenizer: function (d) { return Bloodhound.tokenizers.whitespace(d.word); },
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        limit: 4,
        local: [
          { word: 'Alabama' },
          { word: 'Alaska' },
          { word: 'Arizona' },
          { word: 'Arkansas' },
          { word: 'California' },
          { word: 'Colorado' }
        ]
      });

      states.initialize();

      $('#typeahead-demo-01').typeahead(null, {
        name: 'states',
        displayKey: 'word',
        source: states.ttAdapter()
      });
    }

    // make code pretty
    window.prettyPrint && prettyPrint();
  });
  //end Funtions Flat UI ---------------------------------------------

})(jQuery);
