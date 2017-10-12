angular.
    module('teamDetail').
    component('teamDetail', {
        templateUrl: 'app/teams/team-detail/team-detail.html',
        controller: function ($scope) {
            $scope.members = [
                { 'name': 'Nguyen Van A' },
                { 'name': 'Nguyen Van B' },
                { 'name': 'Nguyen Van C' },
                { 'name': 'Nguyen Van D' },
                { 'name': 'Nguyen Van E' },
                { 'name': 'Nguyen Van F' }
            ];
        }
    })