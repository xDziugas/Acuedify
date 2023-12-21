let timeout = null;
const searchBar = document.getElementById('searchBar');
const searchResultsDiv = document.getElementById('searchResults');
const regularContentDiv = document.getElementById('regularContent');

searchBar.addEventListener('keyup', function (e) {
    if (e.key === 'Enter') {
        performSearch();
    } else {
        clearTimeout(timeout);
        timeout = setTimeout(performSearch, 500);
    }
});

function performSearch() {
    const searchQuery = searchBar.value;
    if (searchQuery.trim() === '') {
        searchResultsDiv.style.display = 'none';
        regularContentDiv.style.display = 'block';
        return;
    }

    fetch(`/Library?handler=Search&query=${encodeURIComponent(searchQuery)}`)
        .then(response => response.text())
        .then(html => {
            searchResultsDiv.innerHTML = html;
            searchResultsDiv.style.display = 'block';
            regularContentDiv.style.display = 'none';
        });
}

document.querySelectorAll('.nav-link-library').forEach(link => {
    link.addEventListener('click', function (e) {
        e.preventDefault();

        document.querySelectorAll('.nav-link-library').forEach(nav => nav.classList.remove('active'));
        document.querySelectorAll('.tab-pane-library').forEach(tab => tab.style.display = 'none');

        this.classList.add('active');
        const activeTabId = this.getAttribute('href');
        const activeTab = document.querySelector(activeTabId);
        activeTab.style.display = 'block';
    });
});

// Initialize first tab as active
document.querySelector('.nav-link-library.active').click();


// Save the current scroll position in local storage
function saveScrollPosition() {
    const scrollPosition = window.scrollY;
    localStorage.setItem('scrollPosition', scrollPosition);
}

window.addEventListener('beforeunload', saveScrollPosition);

// Retrieve the stored scroll position from local storage
function retrieveScrollPosition() {
    return parseInt(localStorage.getItem('scrollPosition')) || 0;
}

const storedScrollPosition = retrieveScrollPosition();
window.scrollTo(0, storedScrollPosition);

//------------------------------------------------------------

document.addEventListener('DOMContentLoaded', function () {
    addFavoriteToggleListeners(document.querySelector('#tests'));
    addFavoriteToggleListeners(document.querySelector('#favs'));
});

function addFavoriteToggleListeners(rootElement) {
    rootElement.querySelectorAll('.toggle-favorite').forEach(item => {
        item.addEventListener('click', async function (e) {
            e.preventDefault();
            var quizId = parseInt(this.getAttribute('data-id'));
            var isFavorite = this.getAttribute('data-isfavorite') == "True";
            var newStatus = !isFavorite;

            await refreshTabContent(quizId, newStatus);

            this.setAttribute('data-isfavorite', newStatus ? 'True' : 'False');

            updateHeartIcon(this, newStatus);

        });
    });
}

function updateHeartIcon(element, isFavorite) {
    const icon = element.querySelector('svg');
    if (icon) {
        if (isFavorite) {
            icon.classList.add('favorite-heart');
            icon.classList.remove('not-favorite-heart');
        } else {
            icon.classList.add('not-favorite-heart');
            icon.classList.remove('favorite-heart');
        }
    }
}

async function refreshTabContent(quizId, newStatus) {
    // Refresh 'tests' tab
    let response = await fetch(`/Library?handler=ReloadTabContent&tab=tests&id=${quizId}&newStatus=${newStatus}`);
    if (!response.ok) {
        throw new Error('Network response was not ok');
    }
    let html = await response.text();
    let testsTab = document.getElementById('tests');
    testsTab.innerHTML = html;
    addFavoriteToggleListeners(testsTab); // Rebind event listeners to 'tests' tab content

    // Refresh 'favs' tab
    response = await fetch(`/Library?handler=ReloadTabContent&tab=favs&id=${quizId}&newStatus=${newStatus}`);
    if (!response.ok) {
        throw new Error('Network response was not ok');
    }
    html = await response.text();
    let favsTab = document.getElementById('favs');
    favsTab.innerHTML = html;
    addFavoriteToggleListeners(favsTab); // Rebind event listeners to 'favs' tab content
}



