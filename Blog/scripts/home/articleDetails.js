requirejs.config({
    baseUrl: '/scripts',
});
require(['common', 'utility/utility'], function (common, util) {

    var articleData = articleModel;
    articleData.CreateTime = util.getCSharpFormatTime(articleData.CreateTime);

    var vm = new Vue({
        el: "#app",
        mounted: function () {
            this.initData();
        },
        data: {
            date: common.date,
            article: articleData,
            cookieUser: cookieUser,
            statistics: [],
            monthArticles: [],
            comments: []
        },
        methods: {
            initData: function () {
                var self = this, comments = articleData.Comments;
                $.when(util.ajax("/Home/GetLayoutData", "GET", {})).done(function (result) {
                    self.statistics = result.Statistics;
                    self.monthArticles = result.WriteMonths;
                }).fail(function (error) {
                    debugger;
                });
                for (var i = 0; i < comments.length; i++) {
                    comments[i].Floor = i + 1;
                    comments[i].CreateTime = util.getCSharpFormatTime(comments[i].CreateTime);
                    self.comments.push(comments[i]);
                }
                common.initCalendar();
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
            manageUser: function () {
                window.location.href = "/Admin/Index";
            },
            addComment: function () {
                var self = this, data = {};
                var isRemember = util.cookieHelper.getCookie("viewerCookie");
                if (!isRemember) {
                    data.userModel = {};
                    data.userModel.name = util.trimString(document.getElementById("name").value);
                    data.userModel.email = util.trimString(document.getElementById("email").value);
                    data.rememberMe = document.getElementById("rememberMe").checked;
                } else {
                    data.rememberMe = true;
                }
                if (data.rememberMe) {
                    self.article.HasCookie = true;
                }
                data.commentModel = {};
                data.commentModel.Content = document.getElementById("commentContent").value;
                data.commentModel.ArticleId = articleModel.Id;
                $.when(util.ajax("/Home/AddComment", "POST", data))
                    .done(function (result) {
                        if (result) {
                            result.CreateTime = util.getCSharpFormatTime(result.CreateTime);
                            result.Floor = self.comments.length + 1;
                            self.comments.push(result);
                            self.$set(self, 'comments', self.comments);
                            $("#commentContent").val("")
                        }
                    })
                    .fail(function (error) {
                        debugger;
                    });
            },
            changeUser: function () {
                var self = this;
                $.when(util.ajax("/Home/RemoveCookie", "POST", {}))
                    .done(function () {
                        self.article.HasCookie = false;
                    }).fail(function (error) {
                        debugger;
                    });
            },
            search: function () {

            }
        }
    });

});