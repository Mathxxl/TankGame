namespace Entities.Player.Upgrades
{
    /// <summary>
    /// Upgrade 01 of the Future World
    /// </summary>
    /// <remarks>
    /// Stage 1 : Improve Defense <br/>
    /// Stage 2 : Improve Defense <br/>
    /// Stage 3 : Improve Defense + May reflect some attacks
    /// </remarks>
    public class FutureUpgrade01 : Upgrade
    {
        protected override void UpgradeObtained(){}

        protected override void UpgradeUpdate(){}

        protected override void UpgradeLevelUp()
        {
            switch (Level)
            {
                case 2:
                    SetupSendBackAttacks();
                    break;
            }
        }
        
        private void SetupSendBackAttacks()
        {
            //TODO : En cas d'OnAttacked, possibilité de renvoyer le projectile (=> donc vraisemblablement désactiver le projectile ennemi et en créer une copie opposée)    
        }
    }
}