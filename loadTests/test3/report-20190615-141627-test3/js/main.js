$(function() {
	var $lineX = $('#chart-line-x');
	var $lineY = $('#chart-line-y');
	var $body  = $('body');

	function updateCrossHair(e) {
		if (!$body.hasClass('chart-line-fixed')) {
			$lineX.css('left', e.pageX );
			$lineY.css('top', e.pageY );
		}
	}

	$body.on('mousemove', updateCrossHair);

	$('#btn-ruler').on('click', function() {
		$lineX.height($('html')[0].scrollHeight);
		$('body').toggleClass('chart-line');
		$('body').removeClass('chart-line-fixed');
	});

	$body.on('click', function(e) {
		if ($body.hasClass('chart-line') && (!$('#settings-toolbar').has(e.target).length)) {
			$('body').toggleClass('chart-line-fixed');
			updateCrossHair(e);
		}
	});

	$('a[data-chart-size]').on('click', function() {
		$body.removeClass('charts-s charts-m charts-l').addClass($(this).data('chartSize'));
		setTimeout(function() {
			$lineX.height($('html')[0].scrollHeight);
		}, 1000);
	});


	$("table.table-sort").tablesorter({
		headers: {
			1: {
				sorter:'number'
			},
			2: {
				sorter:'number'
			},
			3: {
				sorter:'number'
			},
			4: {
				sorter:'number'
			},
			5: {
				sorter:'number'
			},
			6: {
				sorter:'number'
			},
			7: {
				sorter:'number'
			},
			8: {
				sorter:'number'
			},
			9: {
				sorter:'number'
			},
			10: {
				sorter:'number'
			}
		}
	});

	$('table.criterions-list').each(function() {
		var $table = $(this);
		if ($table.find('tr.has-failed, tr.has-passed').length) {
			var $a = $('<a href="javascript:void(0);"></a>');
			var $p = $('<p>Show: </p>');
			$a.on('click', function() {
				if ($table.hasClass('show-failed-only')) {
					$table.removeClass('show-failed-only');
					$(this).text('Failed only');
				} else {
					$table.addClass('show-failed-only');
					$(this).text('All samples');
				}
			});
			$p.append($a);
			$table.before($p);
			$a.trigger('click');
		}
	});
});

$.tablesorter.addParser({
	// set a unique id
	id: 'number',
	is: function(s) {
		// return false so this parser is not auto detected
		return false;
	},
	format: function(s) {
		// format your data for normalization
		return s.toLowerCase().replace(",","").replace(" ","");
	},
	// set type, either numeric or text
	type: 'numeric'
});