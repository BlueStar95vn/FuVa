angular.
    module('teamEdit').
    component('teamEdit', {
        templateUrl: '/app/teams/team-edit/team-edit.html',
        controller: function ($scope) {
            $scope.members = [
                { 'name': 'Nguyen Van A' },
                { 'name': 'Nguyen Van B' },
                { 'name': 'Nguyen Van C' },
                { 'name': 'Nguyen Van D' },
                { 'name': 'Nguyen Van E' },
                { 'name': 'Nguyen Van F' }
            ];

            $scope.addMember = function () {
                $scope.members.push({ 'name': $scope.newMember });
                $scope.newMember = '';

            }

            $scope.delMember = function (index) {
                $scope.members.splice(index, 1);

            }
        }
    });
    