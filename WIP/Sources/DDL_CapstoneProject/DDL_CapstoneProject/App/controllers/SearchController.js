"use strict";

app.controller('SearchController',
    ['$scope', '$http', 'projectbycategory', 'categoryList', 'ProjectService', 'CategoryService', 'isAdvance', 'selectedorder', 'selectcategory', 'searchkey',
    function ($scope, $http, projectbycategory, categoryList, ProjectService, CategoryService, isAdvance, selectedorder, selectcategory, searchkey) {
        //Todo here.
        $scope.projectbycategory = projectbycategory.data.Data;
        $scope.categoryList = [{ "CategoryID": "all", "Name": "Tất cả" }].concat(categoryList.data.Data)
        $scope.projectResultListSize = $scope.projectbycategory.length;
        $scope.isAdvance = isAdvance;
        $scope.selectedorder = selectedorder;
        $scope.searchkey = searchkey;
        for (var i = 0; i < $scope.categoryList.length; i++) {
            if ($scope.categoryList[i].CategoryID == selectcategory) {
                $scope.selectcategory = [$scope.categoryList[i]];
            }
        };
        
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
                $scope.projectResultListSize = $scope.projectbycategory.length;
                if ($scope.projectbycategory.length < 10) {
                    $scope.showLoadMoreButton = false;
                } else {
                    $scope.showLoadMoreButton = true;
                }
            });
        }


        if ($scope.projectbycategory.length < 10) {
            $scope.showLoadMoreButton = false;
        } else {
            $scope.showLoadMoreButton = true;
        }

        $scope.Loadmore = function (index, category, order, searchkey) {
            var categoryIdString = "";
            for (var i = 0; i < category.length; i++) {
                if (category[i].CategoryID == "all") {
                    categoryIdString = "all";
                    break;
                } else {
                    categoryIdString += "|" + category[i].CategoryID + "|";
                }
            }
            var searchResult = ProjectService.SearchProject(index, categoryIdString, order, searchkey).then(function (projectlist) {
                $scope.projectbycategory = $scope.projectbycategory.concat(projectlist.data.Data);
                $scope.projectResultListSize = $scope.projectbycategory.length;
                if (projectlist.data.Data.length < 10) {
                    $scope.showLoadMoreButton = false;
                }
            });
        }
    }]);