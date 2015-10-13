"use strict";

app.service('ProjectService', function ($http) {

    // Function to get a project by ProjectID
    this.getProject = function (projectID) {
        return $http.get("/api/ProjectApi/GetProject/" + projectID);
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
});