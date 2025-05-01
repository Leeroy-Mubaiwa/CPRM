// Tawk.to initialization and customization
document.addEventListener('DOMContentLoaded', function () {
    // Wait for Tawk_API to be available
    if (typeof Tawk_API !== 'undefined') {
        // Set custom attributes
        Tawk_API.onLoad = function () {
            // Set user data if available
            if (window.userData) {
                Tawk_API.setAttributes({
                    'name': window.userData.name,
                    'email': window.userData.email,
                    'user_id': window.userData.userId
                });
            }

            // Add custom styling
            Tawk_API.customStyle = {
                visibility: {
                    desktop: {
                        position: 'br',
                        xOffset: '20px',
                        yOffset: '20px'
                    },
                    mobile: {
                        position: 'br',
                        xOffset: '0px',
                        yOffset: '0px'
                    }
                }
            };

            // Set chat widget language
            Tawk_API.setLanguage('en');

            // Add custom chat messages
            Tawk_API.addTags(['CPRM', 'Partner Portal']);
        };

        // Handle chat events
        Tawk_API.onChatStarted = function () {
            console.log('Chat started');
        };

        Tawk_API.onChatEnded = function () {
            console.log('Chat ended');
        };

        Tawk_API.onChatMessageVisitor = function (message) {
            console.log('Visitor message:', message);
        };

        Tawk_API.onChatMessageAgent = function (message) {
            console.log('Agent message:', message);
        };
    }
}); 