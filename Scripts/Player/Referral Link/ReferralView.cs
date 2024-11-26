using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace GameAssets.Player.Referral
{
    public sealed class ReferralView : MonoBehaviour
    {
        //TODO ссылка на приложение тг (желательно брать с сервера)
        [SerializeField, Required, BoxGroup("Parameters")]
        private string textForQR;

        [SerializeField, Required, BoxGroup("Parameters"), PropertySpace(0, 10)]
        private RawImage qrImage;



        [Button("Copy Link", ButtonSizes.Medium), BoxGroup("Parameters"), HorizontalGroup("Parameters/Link")]
        public void CopyLink()
            => GenerateReferralLink.CopyUserReferralLink();

        [Button("Share", ButtonSizes.Medium), BoxGroup("Parameters"), HorizontalGroup("Parameters/Link")]
        public void Share()
            => GenerateReferralLink.ShareReferralLink();

        [Button("QR", ButtonSizes.Medium), BoxGroup("Parameters"), HorizontalGroup("Parameters/Link")]
        public void QR()
            => qrImage.texture = GenerateReferralLink.GetQR(textForQR);
    }
}
