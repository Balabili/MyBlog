requirejs.config({
    baseUrl: '/scripts',
});
require(['common', 'utility/utility'], function (common, util) {

    var data = [];
    for (var i = 0; i < pageModel.Articles.length; i++) {
        var item = pageModel.Articles[i];
        item.CreateTime = util.getCSharpFormatTime(item.CreateTime);
        item.isTopAritcle = (i == 0);
        data.push(item);
    }

    var vm = new Vue({
        el: "#app",
        mounted: function () {
            this.initData();
        },
        data: {
            date: common.date,
            articles: data,
            pagers: [],
            showPreviousLink: false,
            showNextLink: false,
            statistics: [],
            monthArticles: []
        },
        methods: {
            initData: function () {
                var self = this;
                $.when(util.ajax("/Home/GetLayoutData", "GET", {})).done(function (result) {
                    self.statistics = result.Statistics;
                    self.monthArticles = result.WriteMonths;
                }).fail(function (error) {
                    debugger;
                });
                common.initCalendar();
                self.initPager();
            },
            initPager: function () {
                var pagers = Math.ceil(pageModel.ArticleCount / 10);
                if (pagers > 1) {
                    this.showNextLink = true;
                }
                for (var i = 0; i < pagers; i++) {
                    var pager = {};
                    pager.number = i + 1;
                    pager.isActive = i == 0 ? true : false;
                    this.pagers.push(pager);
                }
            },
            search: function () {
                $.when(util.ajax("/Home/Search")).done(function (result) {

                }).fail(function (error) {

                });
            },
            changePage: function (event) {
                var self = this, filter = {}, index = event.target.innerText, pagerList = $(".pager-container a");
                filter.Pager = {};
                if (index == "<") {
                    filter.Pager.Index = this.caculateIndex(pagerList, false);
                } else if (index == ">") {
                    filter.Pager.Index = this.caculateIndex(pagerList, true);
                } else {
                    filter.Pager.Index = index;
                }
                filter.HasPager = true;
                var params = util.urlHelper.getParameters(), isStatistic = false;
                filter.MainTable = params.maintable;
                filter.Condition = params.condition;
                $.when(util.ajax("/Home/GetArticlesWithCondition", "POST", { filter: filter })).done(function (result) {
                    if (result) {
                        self.articles = [];
                        self.showPreviousLink = false;
                        self.showNextLink = false;
                        for (var i = 0; i < result.length; i++) {
                            result[i].CreateTime = util.getCSharpFormatTime(result[i].CreateTime);
                            result[i].isTopAritcle = (i == 0);
                            self.articles.push(result[i]);
                        }
                        for (var i = 0; i < pagerList.length; i++) {
                            var pager = $(pagerList[i]);
                            pager.removeClass("pager-container-actived");
                            if (filter.Pager.Index == i) {
                                pager.addClass("pager-container-actived");
                            }
                        }
                        if (filter.Pager.Index != 1) {
                            self.showPreviousLink = true;
                        }
                        if (self.pagers.length != filter.Pager.Index) {
                            self.showNextLink = true;
                        }
                        $("html,body").animate({ scrollTop: 0 }, "slow");
                    }
                }).fail(function (error) {
                    debugger;
                });
            },
            caculateIndex: function (pagerList, isAdd) {
                for (var i = 0; i < pagerList.length; i++) {
                    var className = pagerList[i].className;
                    if (className.indexOf("pager-container-actived") > -1) {
                        return isAdd ? i + 1 : i - 1;
                    }
                }
            },
            changeCalendar: function (addMonth) {
                common.changeCalendar(addMonth);
            },
            getArticlesWithStatistic: function (e) {
                var currentElement = e.target.innerText, tag = currentElement.split("(")[0];
                window.location.href = "/Home/Index?maintable=Statistic&condition=" + tag;
            },
            getArticlesWithMonth: function (e) {
                var currentElement = e.target.innerText, reg = /[\u4E00-\u9FA5\uF900-\uFA2D]/, dateList = currentElement.split(reg),
                date = dateList[1] + "/" + dateList[0];
                window.location.href = "/Home/Index?maintable=WriteMonth&condition=" + date;
            },
            viewArticleDetails: function (event) {
                var articleTitle = event.currentTarget.parentElement.parentElement.id;
                window.location.href = "/Home/ArticleDetails?title=" + articleTitle;
            },
            manageUser: function () {
                window.location.href = "/Admin/Index";
            }
        },
    });

});