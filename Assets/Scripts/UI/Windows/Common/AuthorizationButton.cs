using Helpers;
using KillerDoors.Services.WebMediatorSpace;
using UnityEngine;
using UnityEngine.UI;

namespace KillerDoors.UI.Common
{
    public class AuthorizationButton : MonoBehaviour
    {
        [field: SerializeField] public Button Button { get; private set; }
        [field: SerializeField] public AppearingAndDisappearingObject AppearanceDisappearance { get; private set; }

        private IWebMediatorService _webMediatorService;

        public void Construct(IWebMediatorService webMediatorService)
        {
            _webMediatorService = webMediatorService;
#if !UNITY_WEBGL || UNITY_EDITOR
            OnAuthorized();
            return;
#endif
            Button.onClick.AddListener(AuthorizationInYandex);

            if (_webMediatorService.IsAuthorized)
                OnAuthorized();
            else
                _webMediatorService.Authorized += OnAuthorized;
        }
        public void Describes()
        {
            if (_webMediatorService != null)
                _webMediatorService.Authorized -= OnAuthorized;
        }
        private void OnAuthorized()
        {
            AppearanceDisappearance.IgnoreParentControl = true;
            AppearanceDisappearance.StartDisappearing();
        }

        private void AuthorizationInYandex()
        {
            if (_webMediatorService != null)
                _webMediatorService.OpenAuthDialog();
        }
        private void OnDestroy() =>
            Describes();
    }
}