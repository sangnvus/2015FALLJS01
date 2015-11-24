"use strict";

service.service("AdminReportService", function ($http) {

    //Read all Categories for administrator.
    this.GetReportProjects = function () {
        return $http.get("../api/ReportApi/GetReportProjects");
    };
    //Read all Categories for administrator.
    this.GetReportUsers = function () {
        return $http.get("../api/ReportApi/GetReportUsers");
    };

    // Change status of reprot project
    this.changeReportProjectStatus = function (id, status) {
        var request = $http({
            method: 'put',
            url: '../api/ReportApi/changeReportProjectStatus',
            params: {
                id: id,
                status: status
            }
        });
        return request;
    }
    // Change status of reprot project
    this.changeReportUserStatus = function (id, status) {
        var request = $http({
            method: 'put',
            url: '../api/ReportApi/changeReportUserStatus',
            params: {
                id: id,
                status: status
            }
        });
        return request;
    }
});