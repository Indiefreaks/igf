The instancing project is a drop in replacement for the current IGF instancing project, the pipeline needs to be added as a new project

You must set the content processor on the model to 'IGF - Instanced Skinned Model'. There are currently a number of issues with the code

and it will break if the model doesn't use the correct vertex format which is a pretty major bug.


Use is almost identical to the existing instancing.

            InstancedSkinnedModel model = Application.ContentManager.Load<InstancedSkinnedModel>("Models\\SpiderBeast\\EnemyBeast");
            DeferredSasEffect effect = Application.ContentManager.Load<DeferredSasEffect>("Effects\\SkinnedInstancingEffect");

            _instancingManager = Application.SunBurn.GetManager<IInstancingManager>(true);
            SkinnedInstanceFactory spiderFactory = _instancingManager.CreateSkinnedInstanceFactory(model, effect);
            for (int i = 0; i < 100; i++)
            {
                Vector3 position = new Vector3((i / 10) * 10, 0, (i % 10) * 10);
                SkinnedInstanceEntity spiderEntity = spiderFactory.CreateInstance("Spider1", Matrix.CreateScale(0.05f) * Matrix.CreateTranslation(position));
                Application.SunBurn.ObjectManager.Submit(spiderEntity);
                spiderEntity.PlayAnimation("Take 001", true);
            }