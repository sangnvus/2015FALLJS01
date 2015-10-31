"use strict";

app.service('ProjectService', function ($http) {


    //Trungvn

    this.getProjectTop = function (categoryID) {
        return $http.get("/api/ProjectApi/getProjectTop/?categoryID=" + categoryID);
    }

    this.GetProject = function (take, from, categoryid, orderby, searchkey, status, isExprired, isFunded) {
        if (typeof (searchkey) == "undefined") {
            searchkey = "null";
        }
        return $http.get('/api/ProjectApi/GetProject', {
            params:
                {
                    take: take,
                    from: from,
                    categoryid: categoryid,
                    orderby: orderby,
                    searchkey: searchkey,
                    status: status,
                    isExprired: isExprired,
                    isFunded: isFunded
                }
        });
    };
    this.GetProjectStatisticList = function () {
        return $http.get('/api/ProjectApi/GetProjectStatisticList');
    };
    this.GetProjectByCategory = function () {
        return $http.get('/api/ProjectApi/GetProjectByCategory');
    };
    this.GetStatisticListForHome = function () {
        return $http.get('/api/ProjectApi/GetStatisticListForHome');
    };
    this.getStatisticsInfor = function () {
        return $http.get('/api/ProjectApi/getStatisticsInfor');
    };
    this.SearchProject = function (from, categoryidlist, orderby, searchkey) {
        return this.GetProject(12, from, categoryidlist, orderby, searchkey, "", false, "");
    }


    this.SearchCount = function (categoryidlist, searchkey) {
        var request = $http({
            method: 'get',
            url: '/api/ProjectApi/SearchCount',
            params: {
                categoryidlist: categoryidlist,
                searchkey: searchkey
            }
        });

        return request;
    }

    //EndTrungVn



    // Function to get a project by ProjectID
    this.getProject = function (code) {
        var request = $http({
            method: 'get',
            url: '/api/ProjectApi/GetProjectBasic',
            params: {
                code: code
            }
        });

        return request;
    }

    // Function to get backing project information by ProjectID
    this.getBackProjectInfo = function (code) {
        var request = $http({
            method: 'get',
            url: '/api/ProjectApi/GetBackProjectInfo',
            params: {
                code: code
            }
        });

        return request;
    }

    // Function to ger a project by ProjectCode
    this.getProjectDetail = function (code) {
        var request = $http({
            method: 'get',
            url: '/api/ProjectApi/GetProjectDetail',
            params: {
                code: code
            }
        });

        return request;
    }

    // Function to ger a project by ProjectCode
    this.getUpdateLogList = function (code) {
        var request = $http({
            method: 'get',
            url: '/api/ProjectApi/GetUpdateLogList',
            params: {
                code: code
            }
        });

        return request;
    }


    // Function to ger a project by ProjectCode
    this.getCommentList = function (code, lastDatetime) {
        var request = $http({
            method: 'get',
            url: '/api/ProjectApi/GetCommentList',
            params: {
                code: code,
                lastDatetime: lastDatetime
            }
        });

        return request;
    }

    // Function to ger a project by ProjectCode
    //this.loadMoreComment = function (code, skip, lastDatetime) {
    //    var request = $http({
    //        method: 'get',
    //        url: '/api/ProjectApi/GetMoreComment',
    //        params: {
    //            code: code,
    //            skip: skip,
    //            lastDatetime: lastDatetime
    //        }
    //    });

    //    return request;
    //}

    // Function to create new Project
    this.createProject = function (project) {
        var request = $http({
            method: 'post',
            url: '/api/ProjectApi/CreateProject',
            data: project
        });

        return request;
    }

    // Function to edit a Project
    this.editProject = function (file, project) {
        var fdata = new FormData();
        var url = "/api/ProjectApi/EditProjectBasic";

        fdata.append('file', file);
        fdata.append('project', JSON.stringify(project));

        return $http.post(url, fdata, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined },

        })
            .success(function (resp) {
                //debugger;
            })
            .error(function (resp) {
                //debugger;
            });
    }

    // Function to get project's story by ProjectID
    this.getProjectStory = function (projectID) {
        return $http.get("/api/ProjectApi/GetProjectStory/" + projectID);
    }

    // Function to edit project's story
    this.editProjectStory = function (project) {
        var request = $http({
            method: 'put',
            url: '/api/ProjectApi/EditProjectStory/',
            data: project
        });

        return request;
    }

    // Function to get rewardPkgList by ProjectID
    this.getRewardPkgs = function (projectID) {
        return $http.get("/api/ProjectApi/GetRewardPkg/" + projectID);
    }

    // Function to create a rewardPkg
    this.createReward = function (projectId, newRewardPkgs) {
        var request = $http({
            method: 'post',
            url: '/api/ProjectApi/CreateRewardPkg/' + projectId,
            data: newRewardPkgs
        });

        return request;
    }

    // Function to edit rewardPkg
    this.editRewardPkgs = function (editRewardPkg) {
        var request = $http({
            method: 'put',
            url: '/api/ProjectApi/EditRewardPkg/',
            data: editRewardPkg
        });

        return request;
    }

    // Funtion to delete a rewardPkg
    this.deleteRewardPkg = function (rewardPkgID) {
        return $http.delete("/api/ProjectApi/DeleteRewardPkg/" + rewardPkgID);
    }

    // Function to get updateLogList by ProjectID
    this.getUpdateLogs = function (projectID) {
        return $http.get("/api/ProjectApi/GetUpdateLog/" + projectID);
    }

    // Function to create a updateLog
    this.createUpdateLog = function (projectId, newUpdateLog) {
        var request = $http({
            method: 'post',
            url: '/api/ProjectApi/CreateUpdateLog/' + projectId,
            data: newUpdateLog
        });

        return request;
    }

    // Function to edit updateLog
    this.editUpdateLogs = function (editUpdateLog) {
        var request = $http({
            method: 'put',
            url: '/api/ProjectApi/EditUpdateLog/',
            data: editUpdateLog
        });

        return request;
    }

    // Funtion to delete a updateLog
    this.deleteUpdateLog = function (updateLogID) {
        return $http.delete("/api/ProjectApi/DeleteUpdateLog/" + updateLogID);
    }

    // Function to get timeline
    this.getTimeline = function (projectID) {
        return $http.get("/api/ProjectApi/GetTimeLine/" + projectID);
    }

    // Function to create a new timeline point
    this.createTimeline = function (projectId, timeline, file) {
        var fdata = new FormData();
        var url = "/api/ProjectApi/CreateTimeline/" + projectId;

        fdata.append('file', file);
        fdata.append('timeline', JSON.stringify(timeline));

        return $http.post(url, fdata, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }

        })
            .success(function (resp) {
                //debugger;
            })
            .error(function (resp) {
                //debugger;
            });
    }

    // Function to update a timeline point
    this.updateTimeline = function (timeline, file) {
        var fdata = new FormData();
        var url = "/api/ProjectApi/UpdateTimeline/";

        fdata.append('file', file);
        fdata.append('timeline', JSON.stringify(timeline));

        return $http.post(url, fdata, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }

        })
            .success(function (resp) {
                //debugger;
            })
            .error(function (resp) {
                //debugger;
            });
    }

    // Funtion to delete a timeline point
    this.deleteTimeline = function (TimelineID) {
        return $http.delete("/api/ProjectApi/DeleteTimeline/" + TimelineID);
    }

    // Function to get questionList by ProjectID
    this.getQuestion = function (projectID) {
        return $http.get("/api/ProjectApi/GetQuestion/" + projectID);
    }

    // Function to create a question
    this.createQuestion = function (projectId, newQuestion) {
        var request = $http({
            method: 'post',
            url: '/api/ProjectApi/CreateQuestion/' + projectId,
            data: newQuestion
        });

        return request;
    }

    // Function to edit question
    this.editQuestion = function (question) {
        var request = $http({
            method: 'put',
            url: '/api/ProjectApi/EditQuestion/',
            data: question
        });

        return request;
    }

    // Funtion to delete a question
    this.deleteQuestion = function (questionId) {
        return $http.delete("/api/ProjectApi/DeleteQuestion/" + questionId);
    }

    // Function to submit project
    this.submitProject = function (project) {
        var request = $http({
            method: 'put',
            url: '/api/ProjectApi/SubmitProject/',
            data: project
        });

        return request;
    }

    // Function to get rewardPkgs by projectCode
    this.getRewardPkgByCode = function (code) {
        var request = $http({
            method: 'get',
            url: '/api/ProjectApi/GetRewardPkgByCode',
            params: {
                code: code
            }
        });

        return request;
    }

    // Function to get rewardPkgs by projectCode
    this.getRewardPkgByCode = function (code) {
        var request = $http({
            method: 'get',
            url: '/api/ProjectApi/GetRewardPkgByCode',
            params: {
                code: code
            }
        });

        return request;
    }

    // Function to save backing data
    var BackData = {};

    this.addBack = function (newBacking) {
        BackData = newBacking;
    };

    this.getBack = function () {
        return BackData;
    };

    this.return = {
        addBack: this.addBack,
        getBack: this.getBack
    };

    // Function to back project
    this.backingProject = function (backingData) {
        var request = $http({
            method: 'post',
            url: '/api/ProjectApi/BackProject',
            data: backingData
        });

        return request;
    }

    // Function to post comment to server.
    this.Comment = function (code, commment, lastCommentDatetime) {
        var request = $http({
            method: 'post',
            url: '/api/ProjectApi/Comment',
            data: commment,
            params: {
                projectCode: code,
                lastDateTime: lastCommentDatetime
            }
        });

        return request;
    }

    // Function to hide/unhide comment.
    this.ShowHideComment = function (commentID) {
        var request = $http({
            method: 'put',
            url: '/api/ProjectApi/ShowHideComment',
            params: {
                id: commentID
            }
        });

        return request;
    }

    // Function to edit comment.
    this.editComment = function (commentID, content) {
        var request = $http({
            method: 'put',
            url: '/api/ProjectApi/EditComment',
            params: {
                id: commentID,
                content: content
            }
        });

        return request;
    }

    // Function to delete a comment to server.
    this.deleteComment = function (commentID) {
        var request = $http({
            method: 'delete',
            url: '/api/ProjectApi/DeleteComment',
            params: {
                id: commentID
            }
        });

        return request;
    }
// 17/10/2015 - MaiCTP - get BackedProject
    this.getBackedProject = function () {
        var request = $http({
            method: 'get',
            url: '/api/ProjectApi/GetBackedProject/'
          
        });

        return request;

    }

    // 19/10/2015 - MaiCTP - get BackedProjectHistory
    this.getBackedProjectHistory = function () {
        var request = $http({
            method: 'get',
            url: '/api/ProjectApi/GetBackedProjectHistory/'

        });

        return request;

    }

    // 18/10/2015 - MaiCTP - get StarredProject
    this.getStarredProject = function () {
        var request = $http({
            method: 'get',
            url: '/api/ProjectApi/GetStarredProject/'

        });

        return request;

    }

    // 18/10/2015 - MaiCTP - get CreatedProject
    this.getCreatedProject = function () {
        var request = $http({
            method: 'get',
            url: '/api/ProjectApi/GetCreatedProject/',

        });

        return request;

    }

    // 22/10/2015 - MaiCTP - delete RemindedProject
    this.deleteReminded = function (remindedProjectID) {
        return $http.delete("/api/ProjectApi/DeleteProjectReminded/" + remindedProjectID);
    }

    // 24/10/2015 - MaiCTP - DeleteProjectDraft
    this.deleteDraft = function (draftProjectID) {
        return $http.delete("/api/ProjectApi/DeleteProjectDraft/" + draftProjectID);
    }
    // 20/10/2015 - MaiCTP - get RemindedProject
    this.remindProject = function (code) {
        var request = $http({
            method: 'get',
            url: '/api/ProjectApi/RemindProject',
            params: {
                code: code
            }
        });
        return request;
    }

    // 24/10/2015 - MaiCTP - get BackingInfo
    this.backingInfo = function (projectCode) {
        var request = $http({
            method: 'get',
            url: '/api/ProjectApi/GetBackingInfo',
            params: {
                projectCode: projectCode
            }
        });
        return request;
    }

    this.getListBacker = function (code) {
        var request = $http({
            method: 'get',
            url: '/api/ProjectApi/GetListBacker',
            params: {
                code: code
            }
        });
        return request;
    }

    this.reportProject = function (code, reportcontent) {
        var request = $http({
            method: 'get',
            url: '/api/ProjectApi/ReportProject',
            params: {
                code: code,
                content: reportcontent
            }
        });
        return request;
    }
});