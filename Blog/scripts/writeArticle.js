define(['utility/utility'], function (util) {
    KindEditor.ready(function (K) {
        window.editor = K.create('#editor_id', {
            resizeType: 1,
            allowPreviewEmoticons: false,
            allowImageUpload: true,//允许上传图片
            allowFileManager: true, //允许对上传图片进行管理
            uploadJson: '../scripts/editor/asp.net/upload_json.ashx', //上传图片的java代码，只不过放在jsp中
            fileManagerJson: '../scripts/editor/asp.net/file_manager_json.ashx',
            afterUpload: function () { this.sync(); }, //图片上传后，将上传内容同步到textarea中
            afterBlur: function () { this.sync(); },   ////失去焦点时，将上传内容同步到textarea中
        });
    });
    var articleData = {};
    var vm = new Vue({
        el: "#app",
        mounted: function () {
            this.initData();
        },
        data: {
            tagList: ["HTML", "CSS", "JavaScript", "Nodejs", "C#", "MVC"],
            showSaveBtn: true,
            showDraftBtn: false,
            showRestoreBtn: false,
            showDeleteBtn: true
        },
        methods: {
            initData: function () {
                var self = this;
                if (pageModel != null) {
                    document.getElementById("articleTitle").value = pageModel.Title;
                    document.getElementById("articleDescription").value = pageModel.Description;
                    document.getElementById("editor_id").innerHTML = pageModel.Content;
                    document.getElementById("tagList").value = pageModel.Tag;
                    if (pageModel.Status === 0) {
                        self.showDraftBtn = true;
                    } else if (pageModel.Status === 2) {
                        self.showRestoreBtn = true;
                        self.showSaveBtn = false;
                    } else {
                        self.showDraftBtn = false;
                        self.showRestoreBtn = false;
                    }
                } else {
                    self.showDraftBtn = true;
                    self.showDeleteBtn = false;
                }
            },
            saveArticle: function (isDraft) {
                editor.sync();
                if (pageModel != null) {
                    articleData.Id = pageModel.Id;
                    articleData.OldStatus = pageModel.Status;
                }
                articleData.OldStatus = 2;
                articleData.Title = util.trimString(document.getElementById("articleTitle").value);
                articleData.Description = util.trimString(document.getElementById("articleDescription").value);
                articleData.Content = document.getElementById("editor_id").value;
                articleData.Tag = document.getElementById("tagList").value;
                articleData.Status = isDraft ? 0 : 1;
                this.articleValidation(articleData);
            },
            articleValidation: function (articleData) {
                if (articleData.Title == "") {
                    alert("Title can't be empty!");
                }
                $.when(util.ajax("/Admin/TitleValidation", "POST", { title: articleData.Title }))
                    .done(function (result) {
                        if (result) {
                            $.when(util.ajax("/Admin/SaveArticle", "POST", articleData)).done(function (response) {
                                window.location.href = "/Home/Index";
                            }).fail(function (error) {
                                debugger;
                            });
                        } else {
                            alert("The title is repeat,Please change!");
                        }
                    }).fail(function (error) {
                        debugger;
                    });
            },
            restoreArticle: function () {
                var data = {};
                data.id = pageModel.Id;
                data.status = 0;
                $.when(util.ajax("/Admin/ChangeArticleStatus", "POST", data)).done(function (result) {
                    self.statistics = result;
                    window.location.href = "/Admin/Index";
                }).fail(function (error) {
                    debugger;
                });
            },
            deleteArticle: function () {
                var data = {};
                data.id = pageModel.Id;
                data.status = 2;
                $.when(util.ajax("/Admin/ChangeArticleStatus", "POST", data)).done(function (result) {
                    self.statistics = result;
                    window.location.href = "/Admin/Index";
                }).fail(function (error) {
                    debugger;
                });
            },
            cancelToAdminPage: function () {
                window.location.href = "/Admin/Index";
            }
        }
    })
})