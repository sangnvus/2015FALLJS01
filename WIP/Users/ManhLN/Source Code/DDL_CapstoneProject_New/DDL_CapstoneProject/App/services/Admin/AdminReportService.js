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
    this.changeReportStatus = function (id, status, reportType) {
        var request = $http({
            method: 'put',
            url: '../api/ReportApi/changeReportStatus',
            params: {
                id: id,
                status: status,
                reportType: reportType
            }
        });
        return request;
    }
});