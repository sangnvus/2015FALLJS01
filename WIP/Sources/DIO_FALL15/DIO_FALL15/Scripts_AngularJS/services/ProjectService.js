"use strict";

app.service('ProjectService', function ($http) {

    // Read all Projects
    this.getProjects = function () {
        return $http.get("/api/ProjectApi/GetAllProjects");
    }

    // Read all current user's projects
    this.getYourProjects = function () {
        return $http.get("/api/ProjectApi/GetAllCurrentUserProjects");
    }

    // Read all current user's backed projects
    this.getBackedProjects = function () {
        return $http.get("/api/ProjectApi/GetBackedProjects");
    }

    // Read Project by ID
    this.getProject = function (id) {
        return $http.get("/api/ProjectApi/GetProject/" + id);
    }

    // Read all Categories
    this.getCategories = function () {
        return $http.get("/api/ProjectApi/GetAllCategories");
    }

    // Function to create new Project
    this.post = function (Project) {
        var request = $http({
            method: 'post',
            url: '/api/ProjectApi/CreateProject',
            data: Project
        });

        return request;
    }

    // Function to edit a Project
    this.EditProject = function (id, Project) {
        var request = $http({
            method: 'put',
            url: '/api/ProjectApi/EditProject/' + id,
            data: Project
        });

        return request;
    }

    // Function to delete a Project
    this.delete = function (id) {
        var request = $http({
            method: 'delete',
            url: 'api/ProjectApi/DeleteProject/' + id
        });

        return request;
    }

    // Function to upload image file
    this.uploadBulkUserFileToUrl = function (file) {
        var fdata = new FormData();
        var url = "Project/fileUpload";

        fdata.append('file', file);

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

    // Function to back money for a project.
    this.BackProject = function (id, amount) {
        var request = $http({
            method: 'put',
            url: '/api/ProjectApi/BackProject/' + id + '?amount=' + amount,
        });

        return request;
    }
});