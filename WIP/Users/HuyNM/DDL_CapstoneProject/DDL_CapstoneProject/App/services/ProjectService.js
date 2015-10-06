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
        return $http.get("api/ProjectApi/GetProject/" + ProjectID);
    }
});