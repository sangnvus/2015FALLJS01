"use strict";

app.service('ProjectService', function ($http) {

    // Function to create new Project
    this.createProject = function (Project) {
        var request = $http({
            method: 'post',
            url: '/api/ProjectApi/CreateProject',
            data: Project
        });

        return request;
    }

    // Function to edit a Project
    this.editProject = function (ProjectID, Project) {
        var request = $http({
            method: 'put',
            url: '/api/ProjectApi/EditProject/' + ProjectID,
            data: Project
        });

        return request;
    }

    // Function to ger a project by ProjectID
    this.getProject = function (ProjectID) {
        return $http.get("/api/ProjectApi/GetProject/" + ProjectID);
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
    
    // Function to post comment to server.
    this.Comment = function (code,commment) {
        var request = $http({
            method: 'post',
            url: '/api/ProjectApi/Comment',
            data: commment,
            params: {
                projectCode: code
            }
        });

        return request;
    }

    // Function to post comment to server.
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
});