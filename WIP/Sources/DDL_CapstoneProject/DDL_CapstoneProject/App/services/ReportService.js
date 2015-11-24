service.service("ReportService", function ($http) {

    this.reportUser = function (reportedUsername, reportcontent) {
        var request = $http({
            method: 'get',
            url: '/api/ReportApi/ReportUser',
            params: {
                reportedUsername: reportedUsername,
                content: reportcontent
            }
        });
        return request;
    }

    this.reportProject = function (code, reportcontent) {
        var request = $http({
            method: 'get',
            url: '/api/ReportApi/ReportProject',
            params: {
                code: code,
                content: reportcontent
            }
        });
        return request;
    }
});