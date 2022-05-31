let fluxorExperimentsOnKeyDownEventProviderIsActive = true;

window.fluxorExperiments = {
    initializeOnKeyDownEventProvider: function (onKeyDownProviderDisplayReference) {
        document.addEventListener('keydown', (e) => {
            if(!fluxorExperimentsOnKeyDownEventProviderIsActive ||
                e.key.startsWith('F') && e.key.length > 1) {
                return;
            }
            
            e.preventDefault();
            
            let dto = {
                "key": e.key,
                "code": e.code,
                "ctrlWasPressed": e.ctrlKey,
                "shiftWasPressed": e.shiftKey,
                "altWasPressed": e.altKey
            };

            onKeyDownProviderDisplayReference.invokeMethodAsync('DispatchOnKeyDownEventAction', dto);
        });
    },
    setOnKeyDownEventProviderIsActive: function (isActive) {
        fluxorExperimentsOnKeyDownEventProviderIsActive = isActive;
    },
    readClipboard: async function () {
        // First, ask the Permissions API if we have some kind of access to
        // the "clipboard-read" feature.

        try {
            return await navigator.permissions.query({ name: "clipboard-read" }).then(async (result) => {
                // If permission to read the clipboard is granted or if the user will
                // be prompted to allow it, we proceed.

                if (result.state === "granted" || result.state === "prompt") {
                    return await navigator.clipboard.readText().then((data) => {
                        return data;
                    });
                }
                else {
                    return "";
                }
            });
        }
        catch (e) {
            return "";
        }
    },
    setClipboard: function (value) {
        // Copies a string to the clipboard. Must be called from within an
        // event handler such as click. May return false if it failed, but
        // this is not always possible. Browser support for Chrome 43+,
        // Firefox 42+, Safari 10+, Edge and Internet Explorer 10+.
        // Internet Explorer: The clipboard feature may be disabled by
        // an administrator. By default a prompt is shown the first
        // time the clipboard is used (per session).
        if (window.clipboardData && window.clipboardData.setData) {
            // Internet Explorer-specific code path to prevent textarea being shown while dialog is visible.
            return window.clipboardData.setData("Text", text);

        }
        else if (document.queryCommandSupported && document.queryCommandSupported("copy")) {
            var textarea = document.createElement("textarea");
            textarea.textContent = value;
            textarea.style.position = "fixed";  // Prevent scrolling to bottom of page in Microsoft Edge.
            document.body.appendChild(textarea);
            textarea.select();
            try {
                return document.execCommand("copy");  // Security exception may be thrown by some browsers.
            }
            catch (ex) {
                console.warn("Copy to clipboard failed.", ex);
                return false;
            }
            finally {
                document.body.removeChild(textarea);
            }
        }
    }
};