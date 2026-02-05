(function () {

    const content = document.getElementById("spa-content");

    // inject html into body
    async function loadPage(url, pushState = true) {
        content.classList.add("loading");

        const response = await fetch(url, {
            headers: { "X-Requested-With": "XMLHttpRequest" }
        });

        const html = await response.text();
        const doc = new DOMParser().parseFromString(html, "text/html");
        const newContent = doc.querySelector("#spa-content");

        setTimeout(() => {
            content.innerHTML = newContent.innerHTML;
            content.classList.remove("loading");

            runPageScripts(newContent);

            if (pushState) {
                history.pushState({}, "", url);
            }
        }, 200);
    }

    function runPageScripts(content) {
        content.querySelectorAll('script').forEach(oldScript => {
            const newScript = document.createElement('script');

            if (oldScript.src) {
                newScript.src = oldScript.src; // external script
            } else {
                newScript.textContent = oldScript.textContent; // inline script
            }

            document.head.appendChild(newScript).parentNode.removeChild(newScript);
        });
    }

    window.loadPage = loadPage;


})();