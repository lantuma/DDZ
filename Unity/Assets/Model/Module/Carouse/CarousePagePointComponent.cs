using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine;

namespace ETModel
{
    public class CarousePagePointComponent: UIBehaviour
    {
        private ReferenceCollector _rf;

        private GameObject normal0;

        private GameObject normal1;

        private GameObject light0;

        private GameObject light1;

        protected override void Awake()
        {
            base.Awake();

            _rf = GetComponent<ReferenceCollector>();

            normal0 = _rf.Get<GameObject>("0_0");

            normal1 = _rf.Get<GameObject>("1_0");

            light0 = _rf.Get<GameObject>("0_1");

            light1 = _rf.Get<GameObject>("1_1");
        }

        public void ChangeIndex(int curPage)
        {
            if (curPage == 0)
            {
                normal0.SetActive(false);

                normal1.SetActive(true);

                light0.SetActive(true);

                light1.SetActive(false);
            }

            if (curPage == 1)
            {
                normal0.SetActive(true);

                normal1.SetActive(false);

                light0.SetActive(false);

                light1.SetActive(true);
            }
        }
    }
}
