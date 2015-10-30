"use strict";

var searchBlockSize = 12;
app.controller('SearchController',
    ['$scope', '$http', 'projectbycategory', 'categoryList', 'ProjectService', 'CategoryService', 'isAdvance', 'selectedorder', 'selectcategory', 'searchkey', 'projectResultListSize',
function ($scope, $http, projectbycategory, categoryList, ProjectService, CategoryService, isAdvance, selectedorder, selectcategory, searchkey, projectResultListSize) {
    //Todo here.
    $scope.projectbycategory = projectbycategory.data.Data;
    $scope.categoryList = [{ "CategoryID": "all", "Name": "Tất cả" }].concat(categoryList.data.Data)
    $scope.projectResultListSize = projectResultListSize.data.Data;
    $scope.isAdvance = isAdvance;
    $scope.selectedorder = selectedorder;
    $scope.searchkey = searchkey;
    for (var i = 0; i < $scope.categoryList.length; i++) {
        if ($scope.categoryList[i].CategoryID == selectcategory) {
            $scope.selectcategory = [$scope.categoryList[i]];
        }
    };

    $scope.index = searchBlockSize;
    $scope.Search = function (category, order, searchkey) {
        var categoryIdString = "";
        var selectedCategory = angular.copy(category);
        if (!selectedCategory) {
            categoryIdString = "all";
            $scope.selectcategory = [{ "CategoryID": "all", "Name": "Tất cả" }];
        }
        else {
            if ((selectedCategory.length + 1) == $scope.categoryList.length || selectedCategory.length == 0) {
                categoryIdString = "all";
                $scope.selectcategory = [{ "CategoryID": "all", "Name": "Tất cả" }];
            } else {
                for (var i = 0; i < selectedCategory.length; i++) {
                    if (selectedCategory[i].CategoryID == "all") {
                        if (i == selectedCategory.length - 1) {
                            categoryIdString = "all";
                            $scope.selectcategory = [{ "CategoryID": "all", "Name": "Tất cả" }];
                            break;
                        } else {
                            $scope.selectcategory.splice(i, 1);
                            $scope.selectcategory = angular.copy($scope.selectcategory);
                        }
                    } else {
                        categoryIdString += "|" + selectedCategory[i].CategoryID + "|";
                    }
                }
            }
        }
        var searchResult = ProjectService.SearchProject(0, categoryIdString, order, searchkey).then(function (projectlist) {
            $scope.projectbycategory = projectlist.data.Data;
            ProjectService.SearchCount(categoryIdString, searchkey).then(function (response) {
                $scope.projectResultListSize = response.data.Data;
                if ($scope.projectbycategory.length >= $scope.projectResultListSize) {
                    $scope.showLoadMoreButton = false;
                } else {
                    $scope.showLoadMoreButton = true;
                }
            })
            $scope.index = $scope.projectbycategory.length;
        });
    }

    if ($scope.projectbycategory.length >= $scope.projectResultListSize) {
        $scope.showLoadMoreButton = false;
    } else {
        $scope.showLoadMoreButton = true;
    }
    $scope.Loadmore = function (category, order, searchkey) {
        var categoryIdString = "";
        for (var i = 0; i < category.length; i++) {
            if (category[i].CategoryID == "all") {
                categoryIdString = "all";
                break;
            } else {
                categoryIdString += "|" + category[i].CategoryID + "|";
            }
        }
        var searchResult = ProjectService.SearchProject($scope.index, categoryIdString, order, searchkey).then(function (projectlist) {
            $scope.projectbycategory = $scope.projectbycategory.concat(projectlist.data.Data);
            $scope.index = $scope.projectbycategory.length;
            if ($scope.projectbycategory.length >= $scope.projectResultListSize) {
                $scope.showLoadMoreButton = false;
            }
        });
    }
}]);