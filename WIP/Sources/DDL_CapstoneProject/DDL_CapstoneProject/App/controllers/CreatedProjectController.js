//"use strict";

app.controller('CreatedProjectController', function ($scope, projects,$filter) {
    $scope.ListCreatedProject = projects.data.Data;

    $scope.ListCreatedProjectApproved = $filter('filter')($scope.ListCreatedProject, { Status: "approved"});
    $scope.ListCreatedProjectExpired = $filter('filter')($scope.ListCreatedProject, { Status: "expired" });
    $scope.ListCreatedProjectDraft = $filter('filter')($scope.ListCreatedProject, { Status: "draft" });
    $scope.ListCreatedProjectRejected = $filter('filter')($scope.ListCreatedProject, { Status: "rejected" });
   
    $scope.ListCreatedProjectDraft = $scope.ListCreatedProjectDraft.concat($scope.ListCreatedProjectRejected);

    $scope.checkEmptyApproved = $scope.ListCreatedProjectApproved.length;

    $scope.checkEmptyExpired = $scope.ListCreatedProjectExpired.length;
        

    $scope.checkEmptyDraft = $scope.ListCreatedProjectDraft.length;

});
