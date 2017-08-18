define(['utility/utility'], function (util) {

    var date = {}, today = new Date(), statistics = [];
    date.calendarDate = today;
    date.yearMonthFormat = today.getFullYear() + "-" + (today.getMonth() + 1);

    function getYearMonthFormat(today) {
        return today.getFullYear() + "-" + (today.getMonth() + 1);
    }

    function changeCalendar(addMonth) {
        var month = this.date.calendarDate.getMonth();
        if (addMonth) {
            if (month == 11) {
                var year = this.date.calendarDate.getFullYear();
                this.date.calendarDate.setMonth(0);
                this.date.calendarDate.setFullYear(year + 1);
            } else {
                this.date.calendarDate.setMonth(month + 1);
            }
        } else {
            if (month == 0) {
                var year = this.date.calendarDate.getFullYear();
                this.date.calendarDate.setMonth(11);
                this.date.calendarDate.setFullYear(year - 1);
            } else {
                this.date.calendarDate.setMonth(month - 1);
            }
        }
        this.date.yearMonthFormat = getYearMonthFormat(this.date.calendarDate);
        util.initCalendar(this.date.calendarDate);
    }

    function initCalendar() {
        util.initCalendar(new Date());
    }

    return {
        date: date,
        initCalendar: initCalendar,
        changeCalendar: changeCalendar
    }
});

$(function () {
    var _window = $(window);

    $(".has-dropdown-list").mouseover(function () {
        var dropdownListNode = document.getElementById("bDropdownList");
        if (dropdownListNode.className.indexOf("show-nav-dropdown") == -1) {
            $("#bDropdownList").addClass("show-nav-dropdown");
        }
    });

    $(".has-dropdown-list").mouseleave(function () {
        $(".has-dropdown-list ul").removeClass("show-nav-dropdown");
    });

    $(".back-to-top").mouseover(function () {
        $(this).addClass("active-back-to-top");
    });

    $(".back-to-top").mouseleave(function () {
        $(this).removeClass("active-back-to-top");
    });

    _window.scroll(function (event) {
        if (_window.scrollTop() > 100) {
            $("#backTop").css({
                top: _window.scrollTop() + _window.height() - 100 + "px"
            });
        } else {
            $("#backTop").css({ 'bottom': '-50px' });
        }
    });

    $("#backTop").click(function () {
        $("html,body").animate({ scrollTop: 0 }, "slow");
        return false;
    });
})
