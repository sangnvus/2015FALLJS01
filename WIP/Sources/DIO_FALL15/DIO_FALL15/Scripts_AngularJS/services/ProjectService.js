﻿"use strict";

app.service('ProjectService', function ($http) {

    // Read all Projects
    this.getProjects = function () {
        return $http.get("/api/ProjectApi/GetAllProjects");
    }

    // Read Project by ID
    this.getProject = function (id) {
        return $http.get("/api/ProjectApi/GetProject/" + id);
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
    this.put = function (id, Project) {
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
});