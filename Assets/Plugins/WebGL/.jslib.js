mergeInto(LibraryManager.library, {
  IsMobileBrowser: function() {
    return /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent) ? true : false;
  }
});

Module['EXPORTED_FUNCTIONS'] = ['_IsMobileBrowser'];