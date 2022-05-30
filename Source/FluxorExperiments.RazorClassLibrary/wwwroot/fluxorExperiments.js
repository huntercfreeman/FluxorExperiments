let fluxorExperimentsOnKeyDownEventProviderIsActive = true;

window.fluxorExperiments = {
    initializeOnKeyDownEventProvider: function (onKeyDownProviderDisplayReference) {
        document.addEventListener('keydown', (e) => {
            if(!fluxorExperimentsOnKeyDownEventProviderIsActive) {
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
};