angular.module("umbraco").controller("Skybrud.Social.Facebook.OAuth.Controller", ['$scope', 'editorState', function ($scope, editorState) {

    // Define an alias for the editor (eg. used for callbacks)
    var alias = ('skybrudsocial_' + Math.random()).replace('.', '');

    // Get a reference to the current editor state
    var state = editorState.current;

    $scope.expiresDays = null;
    
    $scope.callback = function (data) {

        $scope.model.value = data;

        updateUI();

        updateBusinessPagesDropdownUI();
    };


    function updateBusinessPagesDropdownUI() {

        var array = $scope.model.value.business_pages;
        if (array) {
            //for (var i = 0; i < array.length; i++)
            //  console.log(array[i]);
            $scope.accounts = array;
        }

        //Whether there is a selected account, bind data
        if ($scope.model.value && $scope.model.value.selected_business_page) {
            $scope.selectedBusinessPage = $scope.model.value.selected_business_page;
        }

        //Register onChange event
        $scope.businessPageSelected = function (selectedAccount) {
            $scope.selectedBusinessPage = selectedAccount;
            $scope.model.value.selected_business_page = selectedAccount;//persists on Umbraco model
        };
    }

    $scope.authorize = function () {

        var url = '/App_Plugins/Skybrud.Social/Dialogs/FacebookOAuth.aspx?callback=' + alias;
        url += "&contentTypeAlias=" + state.contentTypeAlias;
        url += "&propertyAlias=" + $scope.model.alias;

        window.open(url, 'Facebook OAuth', 'scrollbars=no,resizable=yes,menubar=no,width=800,height=600');

    };

    $scope.clear = function () {

        $scope.model.value = null;

    };
    
    function format(number, decimals) {
        var text = Math.round(number * 10 * decimals);
        return text / 10 / decimals;
    }
    
    function updateUI() {

        if ($scope.model.value && $scope.model.value.expires_at) {

            var seconds = (new Date($scope.model.value.expires_at) - new Date()) / 1000;
            $scope.expiresDays = format(seconds / 60 / 60 / 24, 2);

        } else {

            $scope.expiresDays = NaN;

        }

    }

    //load stored data in content editor (UI)
    updateUI();
    updateBusinessPagesDropdownUI();

    // Register the callback function in the global scope
    window[alias] = $scope.callback;

}]);

angular.module("umbraco").controller("Skybrud.Social.Facebook.OAuth.PreValues.Controller", ['$scope', 'assetsService', function ($scope, assetsService) {
    
    if (!$scope.model.value) {
        $scope.model.value = {
            appid: '',
            appsecret: '',
            redirecturi: '',
            permissions: []
        };
    }

    //case permissions does not exist (update)
    if (!$scope.model.value.permissions) {
        $scope.model.value.permissions = [];
    }
    
    //https://developers.facebook.com/docs/facebook-login/permissions/v2.4#reference
    $scope.allPermissions = [
        //Permissions That Do Not Require Review
        "public_profile", "user_friends", "email",

        //Extended Profile Properties
        "user_about_me", "user_birthday",
        "user_hometown", "user_likes", "user_location", "user_managed_groups",
        "user_photos", "user_posts", "user_status", "user_videos",
        /*
        "user_education_history", "user_events", "user_games_activity", "user_website", "user_work_history",
        "user_actions.books", "user_actions.fitness", "user_actions.music", "user_actions.news", "user_actions.video",
        */
        //Extended Permissions
        "manage_pages", "publish_pages", "publish_actions"
    ];

    $scope.suggestedRedirectUri = window.location.origin + '/App_Plugins/Skybrud.Social/Dialogs/FacebookOAuth.aspx';

    assetsService.loadCss("/App_Plugins/Skybrud.Social/Social.css");

    $scope.toggleSelection = function toggleSelection(permName) {
        var idx = $scope.model.value.permissions.indexOf(permName);
        
        if (idx > -1) {// is currently selected
            $scope.model.value.permissions.splice(idx, 1);
        }
        else {// is newly selected
            $scope.model.value.permissions.push(permName);
        }
    };
    //TODO: remove all elements after UI loads and then rebuild array only with current checked permissions
}]);