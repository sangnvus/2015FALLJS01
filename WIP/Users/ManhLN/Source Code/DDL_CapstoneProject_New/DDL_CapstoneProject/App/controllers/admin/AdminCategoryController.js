"use strict";

app.controller('AdminCategoryController',
    function ($scope, $rootScope, toastr, listcategory, AdminCategoryService, CommmonService,
        DTOptionsBuilder, DTColumnDefBuilder) {
        //Todo here.
        $scope.ListCategory = listcategory.data.Data;
        $scope.NewCategory = {
            Name: null,
            Description: null
        };

        $scope.EditIndex = null;

        // Define table
        $scope.dtOptions = DTOptionsBuilder.newOptions()
        .withDisplayLength(10)
        .withOption('order', [0, 'asc'])
        .withOption('stateSave', true)
        .withBootstrap();

        $scope.dtColumnDefs = [
            DTColumnDefBuilder.newColumnDef(5).notSortable()
        ];

        $scope.changeCategoryStatus = function (id, index) {
            var promise = AdminCategoryService.changeCategoryStatus(id);
            promise.then(
                function (result) {
                    if (result.data.Status === "success") {
                        $scope.ListCategory[index] = result.data.Data;
                        toastr.success("Thành công");
                    } else {
                        CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                        $scope.Error = result.data.Message;
                        toastr.error($scope.Error, 'Lỗi');
                    }
                },
                function (error) {
                    $scope.Error = error.data.Message;
                });
        }

        // Functions reset new message form
        $scope.resetNewCategoryForm = function resetNewCategoryModel(NewCategoryForm) {
            $scope.NewCategory = {
                Name: null,
                Description: null
            }
            NewCategoryForm.$setPristine(true);
        }

        // Add new category
        $scope.addNewCategory = function () {
            var promisePost = AdminCategoryService.addNewCategory($scope.NewCategory);
            promisePost.then(
                function (result) {
                    if (result.data.Status === "success") {
                        $scope.ListCategory.push(result.data.Data);
                        toastr.success("Tạo danh mục thành công");
                        $('#newCategoryModal').modal('hide');
                    } else {
                        CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                        $scope.NewCategoryError = result.data.Message;
                        toastr.error($scope.NewCategoryError, 'Lỗi');
                    }
                },
                function (error) {
                    $scope.Error = error.data.Message;
                });
        }

        // Delete category
        $scope.deleteCategory = function (id, index) {
            var promise = AdminCategoryService.deleteCategory(id);
            promise.then(
                function (result) {
                    if (result.data.Status === "success") {
                        $scope.ListCategory.splice(index);
                        toastr.success("Xóa thành công");
                    } else {
                        CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                        toastr.error($scope.NewCategoryError, 'Lỗi');
                    }
                },
                function (error) {
                    $scope.Error = error.data.Message;
                });
        }

        $scope.showEditDialog = function (index, EditCategoryForm) {
            $scope.EditIndex = index;
            var category = $scope.ListCategory[index];

            $scope.EditCategory = {
                Name: category.Name,
                Description: category.Description,
                CategoryID: category.CategoryID,
            }

            EditCategoryForm.$setPristine(true);

            $("#EditCategoryModal").modal('show');

        }

        // Edit category
        $scope.editCategory = function () {
            var promise = AdminCategoryService.editCategory($scope.EditCategory);
            promise.then(
                function (result) {
                    if (result.data.Status === "success") {
                        $scope.ListCategory[$scope.EditIndex] = result.data.Data;
                        toastr.success("Sửa thành công");
                        $('#EditCategoryModal').modal('hide');
                    } else {
                        CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                        toastr.error($scope.NewCategoryError, 'Lỗi');
                    }
                },
                function (error) {
                    $scope.Error = error.data.Message;
                });
        }

    });