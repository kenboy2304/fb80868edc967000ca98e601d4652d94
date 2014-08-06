$(document).ready(function() {
	$('.setting, .help, .group-book .book').tooltip({
		'html' : false
	});

	 $('.selectpicker').selectpicker();

	$('button.selectpicker').append('<span class="arrow"><i class="icon-arrow-down5"></i></span>');
	$('button.selectpicker .caret').remove();

	var bookItem = $('.book');
	var bookItemUl = $('.group-book');
	bookItem.click(function(){
		var bookItemParent = $(this).parent('.group-book');
		if($(this).hasClass('active') == false) {
			// Add class active to current li
			bookItem.removeClass('active');
			$(this).addClass('active');

			// get chapter number and name of book from attribute
			var chapter = $(this).attr('data-chapter');
			var bookName = $(this).attr('data-original-title');

			if(bookItemParent.hasClass('current-group') == false) {
				$('.book-chapter').remove();

				// redering element
				var output = '<div class="book-chapter col-md-12" style="display:none">';
				output += '<div class="book-chapter-inner">';
				output += '<span class="arrow-up"></span>';
				output += '<h2 class="book-name">'+ bookName +'</h2>';
				output += '<ul class="clearfix">';
				for(i = 1; i <= chapter; i++) {
					output += '<li class="verse"><a href="#">'+ i +'</a></li>';
				}
				output += '</ul>';
				output += '</div>';
				output += '</div>';

				$(output).appendTo(bookItemParent); //.slideDown();// append to ul
				
				$('.book-chapter').removeAttr('style'); //remove overflow

				bookItemUl.removeClass('current-group');
				bookItemParent.addClass('current-group');
			} else {
				console.log('here');
				$('.book-chapter h2').remove();
				$('.book-chapter ul').remove();

				// redering emelent
				output += '<h2 class="book-name">'+ bookName +'</h2>';
				output += '<ul class="clearfix">';
				for(i = 1; i <= chapter; i++) {
					output += '<li class="verse"><a href="#">'+ i +'</a></li>';
				}

				$(output).appendTo('.book-chapter-inner');
			}
			
			// generate caret
			var offset = $(this).offset();
			var offsetX = offset.left - $('.book-chapter-inner').offset().left;
			
			var arrowLeft = offsetX + 15 + ($(this).width() / 2) + 2;

			$('.arrow-up').css({
				'left' : arrowLeft
			});// end generate
		} else {
			$('.book-chapter').remove();
			$(this).removeClass('active');
			bookItemParent.removeClass('current-group');
		}
	});

    // Reading setting button
    // Pararell
	$('.reading-setting .pararell').click(function () {
	    if ($(this).attr("data-display") == "show") {
	        var html = $('.version').html();
	        html = "<div class='version col-md-6 pararell-content'>" + html + "</div>";
	        $('#reading-content').append(html);
	        $('.version').addClass('col-md-6');
	        $(this).attr("data-display", "hide");
	    } else {
	        $('.version').removeClass('col-md-6');
	        $('.pararell-content').remove();
	        $(this).attr("data-display", "show");
	    }
	    return false;
	});
	// Block vs inline
	$('.reading-setting .block').click(function (){
		$('#reading-content').toggleClass('inline-content');
		$(this).toggleClass('inline');

		return false;
	});
	// Turn light on or of
	$('.reading-setting .light').click(function (){
		$('body').toggleClass('dark');
		$(this).toggleClass('light-off');

		return false;
	});

	// Font size
	var min = 16;
	var max = 20;
	var verse = $('#reading-content .verse');

    var current = 16;

	$('.reading-setting').delegate('.font-add', 'click', function () {
	    
		if(current < max) {
		    current = current + 1;
		    $('#reading-content .verse').css({ 'font-size': current });
		}
		return false;
	});

	$('.reading-setting').delegate('.font-minus', 'click', function () {
	    if (current > min) {
	        current = current - 1;
	        $('#reading-content .verse').css({ 'font-size': current });
	    }
		return false;
	});

	// Reading content
	verse.click(function () {
		//verse.removeClass('selected');
		$(this).toggleClass('selected');
	});

	//
	
		var $window = $(window),
			$searchForm = $('.advanced-form'),
			offset = $searchForm.offset(),
			topPadding = 30;

		$(window).scroll(function() {
			if($window.scrollTop() > offset.top) {
				$searchForm.stop().animate({
					marginTop: $window.scrollTop() - offset.top + topPadding
				}, 400);
			} else {
				$searchForm.stop().animate({
					marginTop: 0
				});	
			}
		});

});