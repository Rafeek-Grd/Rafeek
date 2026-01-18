
(function () {
    // add a simple selector into the topbar
    function addLangSelector() {
        const topbar = document.querySelector('.swagger-ui .topbar');
        if (!topbar) return;

        const wrapper = document.createElement('div');
        wrapper.style.display = 'inline-block';
        wrapper.style.marginLeft = '12px';
        wrapper.style.marginTop = '15px';
        wrapper.style.verticalAlign = 'middle';

        const label = document.createElement('label');
        label.textContent = 'Lang: ';
        label.style.marginRight = '6px';
        label.style.fontSize = '18px';
        label.style.color = 'white';
        wrapper.appendChild(label);

        const select = document.createElement('select');
        select.id = 'swagger-lang-select';
        ['en', 'ar'].forEach(function (l) {
            const o = document.createElement('option');
            o.value = l;
            o.text = l.toUpperCase();
            select.appendChild(o);
        });

        // restore previous selection
        const saved = localStorage.getItem('swagger_lang');
        if (saved) select.value = saved;

        select.addEventListener('change', function () {
            localStorage.setItem('swagger_lang', select.value);
        });

        wrapper.appendChild(select);
        topbar.appendChild(wrapper);
    }

    function installInterceptor(ui) {
        // requestInterceptor sometimes not immediately available - attach when ui is ready
        try {
            const originalConfig = window.ui && window.ui.getConfigs && window.ui.getConfigs();
            if (originalConfig) {
                const prev = originalConfig.requestInterceptor;
                originalConfig.requestInterceptor = function (req) {
                    const lang = localStorage.getItem('swagger_lang');
                    if (lang) {
                        req.headers['Accept-Language'] = lang;
                    }
                    return prev ? prev(req) : req;
                };
            }
        } catch (e) {
            // ignore
        }
    }

    // wait until swagger-ui is available
    const interval = setInterval(() => {
        if (document.querySelector('.swagger-ui .topbar') && window.ui && window.ui.getConfigs) {
            clearInterval(interval);
            addLangSelector();
            installInterceptor(window.ui);
        }
    }, 200);
})();