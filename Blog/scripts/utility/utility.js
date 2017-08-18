define(function () {

    var ajax = function (actionUrl, type, data) {
        var dtd = $.Deferred();
        $.ajax({
            url: actionUrl,
            type: type,
            data: data,
            success: function (result) {
                dtd.resolve(result);
            },
            error: function (error) {
                dtd.reject(error);
            },
        });
        return dtd.promise();
    };

    //--------------------------calendar---------------------------------
    var month = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
    var initCalendar = function (dateTime) {
        var calendarBodyContainer = document.getElementById("calendar-body"),
            calendarBody = document.createDocumentFragment(),
            now = new Date(),
            today = now.getDate(),
            firstDay = _getMonthFirstDay(dateTime),
            allMonthDay = _getAllMonthDay(dateTime),
            date = 1;
        if (calendarBodyContainer.children.length != 0) {
            var childNotesLength = calendarBodyContainer.childNodes.length;
            for (var i = 0; i < childNotesLength; i++) {
                calendarBodyContainer.removeChild(calendarBodyContainer.childNodes[0]);
            }
        }
        for (var i = 0; i < 6; i++) {
            if (date > allMonthDay) {
                break;
            }
            var tr = document.createElement("tr");
            for (var j = 0; j < 7; j++) {
                var td = document.createElement("td");
                if (date > allMonthDay) {
                    break;
                }
                if (i != 0 || j >= firstDay) {
                    td.innerText = date;
                    if (now.getFullYear() == dateTime.getFullYear() && now.getMonth() == dateTime.getMonth() && date == today) {
                        td.className = "color-red";
                    }
                    date++;
                }
                tr.appendChild(td);
            }
            calendarBody.appendChild(tr);
        }
        calendarBodyContainer.appendChild(calendarBody);
    };
    function _getAllMonthDay(date) {
        var day = 0, thisMonth = date.getMonth();
        if (thisMonth == 1) {
            day = _isLeapYear(date) ? 29 : 28;
        } else {
            day = month[thisMonth];
        }
        return day;
    };
    function _isLeapYear(date) {
        var year = date.getFullYear();
        if (year % 100 == 0) {
            return year % 400 == 0;
        } else {
            return year % 4 == 0;
        }
    };
    function _getMonthFirstDay(date) {
        date.setDate(1);
        return date.getDay();
    };

    //--------------------------date format------------------------------
    function getCSharpFormatTime(CsharpDate) {
        var time = getJSDate(CsharpDate);
        return getJsFormatTime(time);
    };
    function getJSDate(CsharpDate) {
        var date = eval('new ' + eval(CsharpDate).source);
        return date;
    };
    function getJsFormatTime(time) {
        return time.getFullYear() + "-" + (time.getMonth() + 1) + "-" + time.getDate() + " " + time.getHours() + ":" + time.getMinutes();
    };

    //------------------------------trim--------------------------------
    function trimString(str) {
        return str.replace(/^\s+|\s+$/g, '');
    }

    //-----------------------------oprate url----------------------------
    var urlHelper = {};

    urlHelper.getParameters = function () {
        var searchString = window.location.search, parameters = {}, paramList = [];
        searchString = searchString.substring(1, searchString.length);
        paramList = searchString.split("&");
        for (var i = 0; i < paramList.length; i++) {
            var param = paramList[i].split("=");
            parameters[param[0]] = param[1];
        }
        return parameters;
    }

    urlHelper.getParameter = function (key) {
        var params = urlHelper.getParameters();
        return params[key];
    }

    //-----------------------------oprate cookie----------------------------
    var cookieHelper = {};

    cookieHelper.setCookie = function (name, value) {
        var Days = 30;
        var exp = new Date();
        exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
        document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString();
    }

    cookieHelper.getCookie = function (name) {
        var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");

        if (arr = document.cookie.match(reg))

            return unescape(arr[2]);
        else
            return null;
    }

    cookieHelper.delCookie = function (name) {
        var exp = new Date();
        exp.setTime(exp.getTime() - 1);
        var cval = getCookie(name);
        if (cval != null)
            document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString();
    }

    return {
        ajax: ajax,
        initCalendar: initCalendar,
        getCSharpFormatTime: getCSharpFormatTime,
        getJsFormatTime: getJsFormatTime,
        trimString: trimString,
        cookieHelper: cookieHelper,
        urlHelper: urlHelper
    };
});