"use strict";

app.controller('SearchController',
    ['$scope', '$http', 'projectbycategory', 'categoryList', 'ProjectService', 'isAdvance', 'selectedorder', 'selectcategory',
    function ($scope, $http, projectbycategory, categoryList, ProjectService, isAdvance, selectedorder, selectcategory) {
        //Todo here.
        $scope.projectbycategory = projectbycategory.data.Data;
        $scope.categoryList = categoryList.data.Data;
        $scope.projectResultListSize = $scope.projectbycategory.length;
        $scope.isAdvance = isAdvance;
        $scope.selectedorder = selectedorder;
        $scope.selectcategory = selectcategory;
        $scope.Search = function (categoryid, order, searchkey) {
            var searchResult = ProjectService.SearchProject("|" + categoryid + "|", order, searchkey).then(function (projectlist) {
                $scope.projectbycategory = projectlist.data.Data;
                $scope.projectResultListSize = $scope.projectbycategory.length;
            });

        }
    }]);