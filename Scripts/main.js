
var Sortable = {
    baseUrl: '',
    sortBy: 0,
    searchTerm: '',
    Search() {
        var searchKey = $('#txtSearch').val();
        window.location.href = Sortable.baseUrl + searchKey;
    },
    Sort(sortby) {
        var isDesc = true;
        const urlParams = new URLSearchParams(window.location.search);

        const isDescOriginal = urlParams.get('isDesc');
        const sortByOriginal = urlParams.get('sortBy');

        if (sortByOriginal != sortBy) {
            if (isDescOriginal == 'true') {
                isDesc = false;
            }
        }

        window.location.href = Sortable.baseUrl + "?sortBy=" + sortBy + "&isDesc" + isDesc;
    }
}