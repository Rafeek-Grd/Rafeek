(function () {
    // add a simple selector into the topbar
    function addLangSelector() {
        const topbar = document.querySelector('.swagger-ui .topbar');
        if (!topbar) return;

        const wrapper = document.createElement('div');
        wrapper.style.cssText = 'display:inline-flex;align-items:center;gap:8px;margin-left:16px;vertical-align:middle;';

        const label = document.createElement('label');
        label.textContent = 'Language';
        label.style.cssText = 'font-size:13px;color:#8b949e;font-weight:500;white-space:nowrap;';
        wrapper.appendChild(label);

        const select = document.createElement('select');
        select.id = 'swagger-lang-select';
        select.style.cssText = [
            'background:#0d1117',
            'color:#e6edf3',
            'border:1px solid #30363d',
            'border-radius:6px',
            'padding:4px 28px 4px 10px',
            'font-size:13px',
            'cursor:pointer',
            'outline:none',
            '-webkit-appearance:none',
            '-moz-appearance:none',
            'appearance:none',
            "background-image:url(\"data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='12' height='12' viewBox='0 0 12 12'%3E%3Cpath fill='%238b949e' d='M2 4l4 4 4-4'/%3E%3C/svg%3E\")",
            'background-repeat:no-repeat',
            'background-position:right 8px center',
            'background-size:12px',
            'transition:border-color .2s ease'
        ].join(';') + ';';

        [{ value: 'en', text: 'English' }, { value: 'ar', text: 'العربية' }].forEach(function (l) {
            const o = document.createElement('option');
            o.value = l.value;
            o.text = l.text;
            select.appendChild(o);
        });

        // restore previous selection
        const saved = localStorage.getItem('swagger_lang');
        if (saved) select.value = saved;

        select.addEventListener('change', function () {
            localStorage.setItem('swagger_lang', select.value);
        });

        select.addEventListener('focus', function () {
            select.style.borderColor = '#58a6ff';
        });

        select.addEventListener('blur', function () {
            select.style.borderColor = '#30363d';
        });

        wrapper.appendChild(select);
        topbar.appendChild(wrapper);
    }

    function installInterceptor() {
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
            installInterceptor();
        }
    }, 200);
})();