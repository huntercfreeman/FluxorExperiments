window.fluxorExperiments = {
    initializeOnKeyDownEventProvider: function (onKeyDownProviderDisplayReference) {
        document.addEventListener('keydown', (e) => {
            if (e.key === "Tab" ||
                (e.key === "a" && e.ctrlKey) ||
                e.key === "ArrowLeft" ||
                e.key === "ArrowDown" ||
                e.key === "ArrowUp" ||
                e.key === "ArrowRight" ||
                e.key === "'") {
                
                e.preventDefault();
            }
            
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
};