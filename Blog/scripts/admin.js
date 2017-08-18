define(['utility/utility'], function (util) {

    var doSomething = function () {
        alert("hello");
    };

    var vm = new Vue({
        el: "#app",
        mounted: function () {
            this.initData();
        },
        data: {
            header: "My Articles",
            userList: [
                { listName: "My Articles", isSelect: true },
                { listName: "My Draft Articles", isSelect: false },
                { listName: "Recycle Bin", isSelect: false },
                { listName: "Manage Comments", isSelect: false },
                { listName: "Configuration", isSelect: false },
                { listName: "Back To Home", isSelect: false },
            ],
            articleList: [],
            showNoItemNotification: false,
            showWriteArticleBtn: true
        },
        methods: {
            initData: function () {
                this.initMyArticlePage();
            },
            changeNav: function (event) {
                var className = event.target.className, tabName = event.target.innerText;
                if (className.indexOf("userlist-is-actived") != -1) {
                    return;
                }
                $("#adminList li").removeClass("userlist-is-actived");
                event.target.className = className + " userlist-is-actived";
                switch (tabName) {
                    case "My Articles":
                        this.initMyArticlePage();
                        this.showWriteArticleBtn = true;
                        break;
                    case "My Draft Articles":
                        this.initMyDraftArticlePage();
                        this.showWriteArticleBtn = false;
                        break;
                    case "Recycle Bin":
                        this.initRecycleBinPage();
                        this.showWriteArticleBtn = false;
                        break;
                    case "Manage Comments":
                        this.initMyArticlePage();
                        this.showWriteArticleBtn = false;
                        break;
                    case "Configuration": break;
                    case "Back To Home":
                        window.location.href = "/Home/Index";
                        break;
                    default: break;
                }
            },
            activeAdminList: function (event) {
                var className = event.target.className;
                if (className.indexOf("active-admin-userlist") == -1) {
                    event.target.className = className + " active-admin-userlist";
                }
            },
            deactiveAdminList: function (event) {
                var classList = event.target.classList;
                for (var i = 0; i < classList.length; i++) {
                    if (classList[i] == "active-admin-userlist") {
                        classList.remove(classList[i]);
                    }
                }
            },
            editArticle: function (articleId) {
                window.location.href = "/Admin/WriteArticle?id=" + articleId;
            },
            submitArticle: function (articleId) {
                $.when(util.ajax("/Admin/SubmitArticle", "POST", { id: articleId }))
                    .done(function () {
                        window.location.reload();
                    }).fail(function (error) {
                        debugger;
                    });
            },
            deleteArticle: function (articleId) {
                var self = this, isDelete = false, articleList = self.articleList;
                for (var i = 0; i < articleList.length; i++) {
                    if (articleList[i].Id == articleId && articleList[i].Status == 2) {
                        isDelete = true;
                        break;
                    }
                }
                debugger;
                $.when(util.ajax("/Admin/DeleteArticle", "POST", { id: articleId, isDelete: isDelete }))
                    .done(function (result) {
                        alert("delete article to recycle bin");
                        debugger;
                        var articleList = self.articleList;
                        for (var i = 0; i < articleList.length; i++) {
                            if (articleList[i].Id == articleId) {
                                articleList.splice(i, 1);
                                break;
                            }
                        }
                    })
                    .fail(function (error) {
                        debugger;
                    });
            },
            restoreArticle: function (articleId) {
                var self = this;
                $.when(util.ajax("/Admin/ChangeArticleStatus", "POST", { id: articleId, status: 0 }))
                    .done(function (result) {
                        alert("restore article to recycle bin");
                        var articleList = self.articleList;
                        for (var i = 0; i < articleList.length; i++) {
                            if (articleList[i].Id == articleId) {
                                articleList.splice(i, 1);
                                break;
                            }
                        }
                    })
                    .fail(function (error) {
                        debugger;
                    });
            },
            writeArticle: function () {
                window.location.href = "/Admin/WriteArticle";
            },
            initMyArticlePage: function () {
                var self = this;
                $.when(util.ajax("/Admin/GetArticles?status=1"), "GET", {})
                    .done(function (result) {
                        self.initArticleItems(result);
                    })
                    .fail(function (error) {
                        debugger;
                    });
            },
            initMyDraftArticlePage: function () {
                var self = this;
                $.when(util.ajax("/Admin/GetArticles?status=0"), "GET", {})
                    .done(function (result) {
                        self.initArticleItems(result);
                    })
                    .fail(function (error) {
                        debugger;
                    });
            },
            initRecycleBinPage: function () {
                var self = this;
                $.when(util.ajax("/Admin/GetArticles?status=2"), "GET", {})
                    .done(function (result) {
                        self.initArticleItems(result);
                    })
                    .fail(function (error) {
                        debugger;
                    });
            },
            initArticleItems: function (result) {
                this.articleList = [], this.showNoItemNotification = false;
                if (result.length == 0) {
                    this.showNoItemNotification = true;
                }
                for (var i = 0; i < result.length; i++) {
                    result[i].CreateTime = util.getCSharpFormatTime(result[i].CreateTime);
                    this.articleList.push(result[i]);
                }
            }
        }
    });
});