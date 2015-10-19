"use strict";

app.service('ProjectService', function ($http) {

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

    // Function to ger a project by ProjectCode
    this.getProjectDetail = function (code) {
        var request = $http({
            method: 'get',
            url: '/api/ProjectApi/getProjectDetail',
            params: {
                code: code
            }
        });

        return request;
    }

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

    this.GetProject = function (take, categoryid, orderby) {
        return $http.get('/api/ProjectApi/GetProject/', {
            params:
                {
                    categoryid: categoryid,
                    take: take,
                    orderby: orderby
                }
        });
    };

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

    this.GetProjectStatisticList = function () {
        return $http.get('/api/ProjectApi/GetProjectStatisticList');
    };
    this.GetProjectByCategory = function () {
        return $http.get('/api/ProjectApi/GetProjectByCategory');
    };
    this.GetStatisticListForHome = function () {
        return $http.get('/api/ProjectApi/GetStatisticListForHome');
    };

    // Function to post comment to server.
    this.Comment = function (code, commment, lastCommentDatetime) {
        var request = $http({
            method: 'post',
            url: '/api/ProjectApi/Comment',
            data: commment,
            params: {
                projectCode: code,
                lastComment: lastCommentDatetime
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
            url: '/api/ProjectApi/GetCreatedProject/'

        });

        return request;

    }

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
});