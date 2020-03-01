(function ($) {
    "use strict";
    $.fn.extend({
        monthly: function (customOptions) {

            // These are overridden by options declared in footer
            var defaults = {
                dataType: "json",
                disablePast: false,
                eventList: true,
                events: customOptions.events,
                jsonUrl: "",
                linkCalendarToEventUrl: false,
                maxWidth: false,
                mode: "event",
                setWidth: false,
                showTrigger: "",
                startHidden: false,
                stylePast: false,
                target: "",
                useIsoDateFormat: false,
                weekStart: 1,	// Monday
                xmlUrl: "",
                listaDatuma: customOptions.listaDatuma
            };

            var options = $.extend(defaults, customOptions),
                uniqueId = $(this).attr("id"),
                parent = "#" + uniqueId,
                currentDate = new Date(),
                currentMonth = currentDate.getMonth() + 1,
                currentYear = currentDate.getFullYear(),
                currentDay = currentDate.getDate(),
                locale = (options.locale || defaultLocale()).toLowerCase(),
                monthNameFormat = options.monthNameFormat || "short",
                weekdayNameFormat = options.weekdayNameFormat || "short",
                monthNames = options.monthNames || defaultMonthNames(),
                dayNames = options.dayNames || defaultDayNames(),
                markupBlankDay = '<div class="m-d monthly-day-blank"><div class="monthly-day-number"></div></div>',
                weekStartsOnMonday = options.weekStart === "Pon" || options.weekStart === 1 || options.weekStart === "1",
                primaryLanguageCode = locale.substring(0, 2).toLowerCase();

            if (options.maxWidth !== false) {
                $(parent).css("maxWidth", options.maxWidth);
            }
            if (options.setWidth !== false) {
                $(parent).css("width", options.setWidth);
            }

            if (options.startHidden) {
                $(parent).addClass("monthly-pop").css({
                    display: "none",
                    position: "absolute"
                });
                $(document).on("focus", String(options.showTrigger), function (event) {
                    $(parent).show();
                    event.preventDefault();
                });
                $(document).on("click", String(options.showTrigger) + ", .monthly-pop", function (event) {
                    event.stopPropagation();
                    event.preventDefault();
                });
                $(document).on("click", function () {
                    $(parent).hide();
                });
            }

            // Add Day Of Week Titles
            _appendDayNames(weekStartsOnMonday);

            // Add CSS classes for the primary language and the locale. This allows for CSS-driven
            // overrides of the language-specific header buttons. Lowercased because locale codes
            // are case-insensitive but CSS is not.
            $(parent).addClass("monthly-locale-" + primaryLanguageCode + " monthly-locale-" + locale);

            // Add Header & event list markup
            $(parent).prepend('<div class="monthly-header"><div class="monthly-header-title"><a href="#" class="monthly-header-title-date" onclick="return false"></a></div><a href="#" class="monthly-prev"></a><a href="#" class="monthly-next"></a></div>').append('<div class="monthly-event-list"></div>');

            // Set the calendar the first time
            setMonthly(currentMonth, currentYear);

            // How many days are in this month?
            function daysInMonth(month, year) {
                return month === 2 ? (year & 3) || (!(year % 25) && year & 15) ? 28 : 29 : 30 + (month + (month >> 3) & 1);
            }




            // Build the month
            function setMonthly(month, year) {
                $(parent).data("setMonth", month).data("setYear", year);

                //	console.log(month, year);

                // Get number of days
                var index = 0,
                    dayQty = daysInMonth(month, year),
                    // Get day of the week the first day is
                    mZeroed = month - 1,
                    firstDay = new Date(year, mZeroed, 1, 0, 0, 0, 0).getDay(),
                    settingCurrentMonth = month === currentMonth && year === currentYear;

                // Remove old days
                $(parent + " .monthly-day, " + parent + " .monthly-day-blank").remove();
                $(parent + " .monthly-event-list, " + parent + " .monthly-day-wrap").empty();
                // Print out the days
                for (var dayNumber = 1; dayNumber <= dayQty; dayNumber++) {
                    // Check if it's a day in the past
                    var isInPast = options.stylePast && (
                        year < currentYear
                        || (year === currentYear && (
                            month < currentMonth
                            || (month === currentMonth && dayNumber < currentDay)
                        ))),
                        innerMarkup = '<div class="monthly-day-number">' + dayNumber + '</div><div class="monthly-indicator-wrap"></div>';
                    if (options.mode === "event") {
                        //console.log(dayNumber, month, year);
                        var thisDate = new Date(year, mZeroed, dayNumber, 0, 0, 0, 0);

                        //ovdje nastaviti za datum


                        $(parent + " .monthly-day-wrap").append("<div"
                            + attr("class", "m-d monthly-day monthly-day-event grow"
                                + (isInPast ? " monthly-past-day" : "")
                                + " dt" + thisDate.toISOString().slice(0, 10)
                            )
                            + attr("data-number", dayNumber)
                            + attr("style", checkDate(dayNumber, month, year, false))
                            + attr("data-tippy-content", checkDate(dayNumber, month, year, true))
                            + ">" + innerMarkup + "</div>");
                        //console.log(dayNumber, month, year);
                        $(parent + " .monthly-event-list").append("<div"
                            + attr("class", "monthly-list-item")
                            + attr("id", uniqueId + "day" + dayNumber)
                            + attr("data-number", dayNumber)

                            + '><div class="monthly-event-list-date">' + dayNames[thisDate.getDay()] + "<br>" + dayNumber + "</div></div>");

                        tippy('.grow', {
                            arrow: true,
                            arrowType: 'round',
                        });

                    } else {
                        $(parent + " .monthly-day-wrap").append("<a"
                            + attr("href", "#")
                            + attr("class", "m-d monthly-day monthly-day-pick" + (isInPast ? " monthly-past-day" : ""))
                            + attr("data-number", dayNumber)
                            + ">" + innerMarkup + "</a>");
                    }
                }

                if (settingCurrentMonth) {
                    $(parent + ' *[data-number="' + currentDay + '"]').addClass("monthly-today");
                }

                // Reset button
                $(parent + " .monthly-header-title").html('<a href="#" class="monthly-header-title-date" onclick="return false">' + monthNames[month - 1] + " " + year + "</a>" + (settingCurrentMonth && $(parent + " .monthly-event-list").hide() ? "" : '<a href="#" class="monthly-reset"></a>'));

                // Account for empty days at start
                if (weekStartsOnMonday) {
                    if (firstDay === 0) {
                        _prependBlankDays(6);
                    } else if (firstDay !== 1) {
                        _prependBlankDays(firstDay - 1);
                    }
                } else if (firstDay !== 7) {
                    _prependBlankDays(firstDay);
                }

                // Account for empty days at end
                var numdays = $(parent + " .monthly-day").length,
                    numempty = $(parent + " .monthly-day-blank").length,
                    totaldays = numdays + numempty,
                    roundup = Math.ceil(totaldays / 7) * 7,
                    daysdiff = roundup - totaldays;
                if (totaldays % 7 !== 0) {
                    for (index = 0; index < daysdiff; index++) {
                        $(parent + " .monthly-day-wrap").append(markupBlankDay);
                    }
                }

                // Events
                if (options.mode === "event") {
                    addEvents(month, year);
                }
                var divs = $(parent + " .m-d");
                for (index = 0; index < divs.length; index += 7) {
                    divs.slice(index, index + 7).wrapAll('<div class="monthly-week"></div>');
                }
            }

            //check for date
            function checkDate(dayNumber2, month2, year2, backText) {
                for (let index = 0; index < defaults.listaDatuma.datumi.length; index++) {
                    const datumPocetka = defaults.listaDatuma.datumi[index].pocetak;
                    const datumKraja = defaults.listaDatuma.datumi[index].kraj;
                    const tipID = defaults.listaDatuma.datumi[index].tipID;

                    var dateCheck = month2 + "." + dayNumber2 + "." + year2;
                    var from = Date.parse(datumPocetka);
                    var to = Date.parse(datumKraja);
                    var check = Date.parse(dateCheck);

                    if ((check <= to && check >= from)) {
                        return textOrStyle(tipID, backText);
                    }

                }
                if (backText === true) return "Nema informacija";
                else return "background-color:white";
            }

            function textOrStyle(tipID, backText) {
                if (tipID == 1) {
                    if (backText === true) return "Na poslu";
                    else return "background-color:#A3CB38";
                }
                else if (tipID == 2) {
                    if (backText === true) return "Privatno odsutan";
                    else return "background-color:#F1C40F";
                }
                else if (tipID == 3) {
                    if (backText === true) return "Višednevno odsutan";
                    else return "background-color:#00537A";
                }
                else if (tipID == 4) {
                    if (backText === true) return "Nepoznato";
                    else return "background-color:#E74C3C";
                }
                else if (tipID == 5) {
                    if (backText === true) return "Službeno odsutan";
                    else return "background-color:#ffff1a";
                }
                else if (tipID == 6) {
                    if (backText === true) return "Neradni dan";
                    else return "background-color:#a6a6a6";
                }
                else {
                    if (backText === true) return "Nema";
                    else return "background-color:white";
                }
            }

            function addEvent(event, setMonth, setYear) {
                // Year [0]   Month [1]   Day [2]
                var fullStartDate = _getEventDetail(event, "startdate"),
                    fullEndDate = _getEventDetail(event, "enddate"),
                    startArr = fullStartDate.split("-"),
                    startYear = parseInt(startArr[0], 10),
                    startMonth = parseInt(startArr[1], 10),
                    startDay = parseInt(startArr[2], 10),
                    startDayNumber = startDay,
                    endDayNumber = startDay,
                    showEventTitleOnDay = startDay,
                    startsThisMonth = startMonth === setMonth && startYear === setYear,
                    happensThisMonth = startsThisMonth;

                if (fullEndDate) {
                    // If event has an end date, determine if the range overlaps this month
                    var endArr = fullEndDate.split("-"),
                        endYear = parseInt(endArr[0], 10),
                        endMonth = parseInt(endArr[1], 10),
                        endDay = parseInt(endArr[2], 10),
                        startsInPastMonth = startYear < setYear || (startMonth < setMonth && startYear === setYear),
                        endsThisMonth = endMonth === setMonth && endYear === setYear,
                        endsInFutureMonth = endYear > setYear || (endMonth > setMonth && endYear === setYear);
                    if (startsThisMonth || endsThisMonth || (startsInPastMonth && endsInFutureMonth)) {
                        happensThisMonth = true;
                        startDayNumber = startsThisMonth ? startDay : 1;
                        endDayNumber = endsThisMonth ? endDay : daysInMonth(setMonth, setYear);
                        showEventTitleOnDay = startsThisMonth ? startDayNumber : 1;
                    }
                }
                if (!happensThisMonth) {
                    return;
                }

                var startTime = _getEventDetail(event, "starttime"),
                    timeHtml = "",
                    eventURL = _getEventDetail(event, "url"),
                    eventTitle = _getEventDetail(event, "name"),
                    eventClass = _getEventDetail(event, "class"),
                    eventColor = _getEventDetail(event, "color"),
                    eventId = _getEventDetail(event, "id"),
                    customClass = eventClass ? " " + eventClass : "",
                    dayStartTag = "<div",
                    dayEndTags = "</span></div>";

                if (startTime) {
                    var endTime = _getEventDetail(event, "endtime");
                    timeHtml = '<div><div class="monthly-list-time-start">' + formatTime(startTime) + "</div>"
                        + (endTime ? '<div class="monthly-list-time-end">' + formatTime(endTime) + "</div>" : "")
                        + "</div>";
                }

                if (options.linkCalendarToEventUrl && eventURL) {
                    dayStartTag = "<a" + attr("href", eventURL);
                    dayEndTags = "</span></a>";
                }

                var markupDayStart = dayStartTag
                    + attr("data-eventid", eventId)
                    + attr("title", eventTitle)
                    // BG and FG colors must match for left box shadow to create seamless link between dates
                    + (eventColor ? attr("style", "background:" + eventColor + ";color:" + eventColor) : ""),
                    markupListEvent = "<a"
                        + attr("href", eventURL)
                        + attr("class", "listed-event" + customClass)
                        + attr("data-eventid", eventId)
                        + (eventColor ? attr("style", "background:" + eventColor) : "")
                        + attr("title", eventTitle)
                        + ">" + eventTitle + " " + timeHtml + "</a>";
                for (var index = startDayNumber; index <= endDayNumber; index++) {
                    var doShowTitle = index === showEventTitleOnDay;
                    // Add to calendar view
                    $(parent + ' *[data-number="' + index + '"] .monthly-indicator-wrap').append(
                        markupDayStart
                        + attr("class", "monthly-event-indicator" + customClass
                            // Include a class marking if this event continues from the previous day
                            + (doShowTitle ? "" : " monthly-event-continued")
                        )
                        + "><span>" + (doShowTitle ? eventTitle : "") + dayEndTags);
                    // Add to event list
                    $(parent + ' .monthly-list-item[data-number="' + index + '"]')
                        .addClass("item-has-event")
                        .append(markupListEvent);
                }
            }

            function addEvents(month, year) {
                if (options.events) {
                    // Prefer local events if provided
                    addEventsFromString(options.events, month, year);
                } else {
                    var remoteUrl = options.dataType === "xml" ? options.xmlUrl : options.jsonUrl;
                    if (remoteUrl) {
                        // Replace variables for month and year to load from dynamic sources
                        var url = String(remoteUrl).replace("{month}", month).replace("{year}", year);
                        $.get(url, { now: $.now() }, function (data) {
                            addEventsFromString(data, month, year);
                        }, options.dataType).fail(function () {
                            console.error("Monthly.js failed to import " + remoteUrl + ". Please check for the correct path and " + options.dataType + " syntax.");
                        });
                    }
                }
            }

            function addEventsFromString(events, setMonth, setYear) {
                if (options.dataType === "xml") {
                    $(events).find("event").each(function (index, event) {
                        addEvent(event, setMonth, setYear);
                    });
                } else if (options.dataType === "json") {
                    $.each(events.monthly, function (index, event) {
                        addEvent(event, setMonth, setYear);
                    });
                }
            }

            function attr(name, value) {
                var parseValue = String(value);
                var newValue = "";
                for (var index = 0; index < parseValue.length; index++) {
                    switch (parseValue[index]) {
                        case "'": newValue += "&#39;"; break;
                        case "\"": newValue += "&quot;"; break;
                        case "<": newValue += "&lt;"; break;
                        case ">": newValue += "&gt;"; break;
                        default: newValue += parseValue[index];
                    }
                }
                return " " + name + "=\"" + newValue + "\"";
            }

            function _appendDayNames(startOnMonday) {
                var offset = startOnMonday ? 1 : 0,
                    dayName = "",
                    dayIndex = 0;
                for (dayIndex = 0; dayIndex < 6; dayIndex++) {
                    dayName += "<div>" + dayNames[dayIndex + offset] + "</div>";
                }
                dayName += "<div>" + dayNames[startOnMonday ? 0 : 6] + "</div>";
                $(parent).append('<div class="monthly-day-title-wrap">' + dayName + '</div><div class="monthly-day-wrap"></div>');
            }

            // Detect the user's preferred language
            function defaultLocale() {
                if (navigator.languages && navigator.languages.length) {
                    return navigator.languages[0];
                }
                return navigator.language || navigator.browserLanguage;
            }

            // Use the user's locale if possible to obtain a list of short month names, falling back on English
            function defaultMonthNames() {
              
                return ["Januar", "Februar", "Mart", "April", "Maj", "Jun", "Jul", "Avgust", "Septembar", "Oktobar", "Novembar", "Decembar"];
                
            }

            // Use the user's locale if possible to obtain a list of short weekday names, falling back on English
            function defaultDayNames() {
               
                return ["Ned", "Pon", "Uto", "Sri", "Čet", "Pet", "Sub"];
               



                
            }

            function _prependBlankDays(count) {
                var wrapperEl = $(parent + " .monthly-day-wrap"),
                    index = 0;
                for (index = 0; index < count; index++) {
                    wrapperEl.prepend(markupBlankDay);
                }
            }

            function setNextMonth() {
                var setMonth = $(parent).data("setMonth"),
                    setYear = $(parent).data("setYear"),
                    newMonth = setMonth === 12 ? 1 : setMonth + 1,
                    newYear = setMonth === 12 ? setYear + 1 : setYear;
                setMonthly(newMonth, newYear);
                viewToggleButton();
            }

            function setPreviousMonth() {
                var setMonth = $(parent).data("setMonth"),
                    setYear = $(parent).data("setYear"),
                    newMonth = setMonth === 1 ? 12 : setMonth - 1,
                    newYear = setMonth === 1 ? setYear - 1 : setYear;
                setMonthly(newMonth, newYear);
                viewToggleButton();
            }

            function viewToggleButton() {
                if ($(parent + " .monthly-event-list").is(":visible")) {
                    $(parent + " .monthly-cal").remove();
                    $(parent + " .monthly-header-title").prepend('<a href="#" class="monthly-cal"></a>');
                }
            }

            //popravka modala
            $('#exampleModal').on("hidden.bs.modal", function () {
                $(document.body).unbind("click", setNextMonth());
                $(document.body).unbind("click", setPreviousMonth());
                $('a[href="#list-detalji"]').click();
            });

            // Advance months  -  mjesec poslije
            $(document.body).on("click", parent + " .monthly-next", function (event) {
                setNextMonth();
                event.preventDefault();
            });

            // Go back in months  -  mjesec prije
            $(document.body).on("click", parent + " .monthly-prev", function (event) {
                setPreviousMonth();
                event.preventDefault();
            });

            // Reset Month - vraca na trenutni mjesec
            $(document.body).on("click", parent + " .monthly-reset", function (event) {
                $(this).remove();
                setMonthly(currentMonth, currentYear);
                viewToggleButton();
                event.preventDefault();
                event.stopPropagation();
            });
        }
    });
}(jQuery));