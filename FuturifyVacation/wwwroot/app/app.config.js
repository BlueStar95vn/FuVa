angular.
    module('vacationApp').
    config(['$locationProvider', '$routeProvider',
        function config($locationProvider, $routeProvider) {
            $locationProvider.hashPrefix("");
            $routeProvider.
                when('/login', {
                    template: '<login></login>',
                    restrictions: {
                        ensureAuthenticated: false,
                        loginRedirect: true
                    }
                }).
                when('/logout', {
                    template: '',
                    controller: 'authLogoutController',
                    controllerAs: 'logoutCtrl',
                    restrictions: {
                        ensureAuthenticated: true,
                        loginRedirect: false
                    }
                }).
                when('/status', {
                    templateUrl: 'app/Auth/auth.status.view.html',
                    controller: 'authStatusController',
                    controllerAs: 'authStatusCtrl',
                    restrictions: {
                        ensureAuthenticated: true,
                        loginRedirect: false
                    }
                }).
                when('/home', {
                    template: '<home></home>',
                    restrictions: {
                        ensureAuthenticated: true,
                        loginRedirect: false
                    }
                }).
                when('/profile', {
                    template: '<profile-detail></profile-detail>',
                    restrictions: {
                        ensureAuthenticated: true,
                        loginRedirect: false
                    }
                }).
                when('/profile/edit', {
                    template: '<profile-edit></profile-edit>',
                    restrictions: {
                        ensureAuthenticated: true,
                        loginRedirect: false
                    }
                }).
                when('/change-password', {
                    template: '<change-password></change-password>',
                    restrictions: {
                        ensureAuthenticated: true,
                        loginRedirect: false
                    }
                }).
                when('/employees', {
                    template: '<employee-list></employee-list>',
                    restrictions: {
                        ensureAuthenticated: true,
                        loginRedirect: false
                    }
                }).
                when('/employees/detail/:userId', {
                    template: '<employee-detail></employee-detail>',
                    restrictions: {
                        ensureAuthenticated: true,
                        loginRedirect: false
                    }
                }).
                when('/employees/edit/:userId', {
                    template: '<employee-edit></employee-edit>',
                    restrictions: {
                        ensureAuthenticated: true,
                        loginRedirect: false
                    }
                }).
                when('/employees/add', {
                    template: '<employee-add></employee-add>',
                    restrictions: {
                        ensureAuthenticated: true,
                        loginRedirect: false
                    }
                }).
                when('/teams', {
                    template: '<team-list></team-list>',
                    restrictions: {
                        ensureAuthenticated: true,
                        loginRedirect: false
                    }
                }).
                when('/teams/detail/:teamId', {
                    template: '<team-detail></team-detail>',
                    restrictions: {
                        ensureAuthenticated: true,
                        loginRedirect: false
                    }
                }).
                when('/teams/edit/:teamId', {
                    template: '<team-edit><team-edit>',
                    restrictions: {
                        ensureAuthenticated: true,
                        loginRedirect: false
                    }
                }).
                when('/teams/add', {
                    template: '<team-add></team-add>',
                    restrictions: {
                        ensureAuthenticated: true,
                        loginRedirect: false
                    }
                }).
                when('/vacation', {
                    template: '<vacation></vacation>',
                    restrictions: {
                        ensureAuthenticated: true,
                        loginRedirect: false
                    }
                }).
                when('/vacation/request', {
                    template: '<vacation-request></vacation-request>',
                    restrictions: {
                        ensureAuthenticated: true,
                        loginRedirect: false
                    }
                }).
                when('/report', {
                    template: '<report></report>',
                    restrictions: {
                        ensureAuthenticated: true,
                        loginRedirect: false
                    }
                }).
                when('/setting', {
                    template: '<setting></setting>',
                    restrictions: {
                        ensureAuthenticated: true,
                        loginRedirect: false
                    }
                }).
                otherwise({
                    redirectTo: '/home',
                    restrictions: {
                        ensureAuthenticated: true,
                        loginRedirect: false
                    }
                });
        }
    ]).
    run(function routeStart($rootScope, $location, $route, authService) {

        $rootScope.location = $location;
        $rootScope.$on('$routeChangeStart', (event, next, current) => {

            if (next.restrictions.ensureAuthenticated) {
                authService.ensureAuthenticated().then(function (response) {
                    if (response.data.role != 'ADMIN' && (next.$$route.originalPath == "/employees"
                        || next.$$route.originalPath == "/employees/detail/:userId" || next.$$route.originalPath == "/employees/add"
                        || next.$$route.originalPath == "/employees/edit/:userId" || next.$$route.originalPath == "/teams"
                        || next.$$route.originalPath == "/teams/edit/:teamId"
                        || next.$$route.originalPath == "/teams/add" || next.$$route.originalPath == "/vacation/request"
                        || next.$$route.originalPath == "/report" || next.$$route.originalPath == "/setting")) {
                        $location.path('/home');
                    }

                }).catch(function (err) {
                    console.log(err);
                    $location.path('/login');
                });
            }
            if (next.restrictions.loginRedirect) {
                authService.ensureAuthenticated().then(function () {
                    $location.path('/home');
                }).catch(function (err) {
                    console.log(err);
                });
            }
        })
    });