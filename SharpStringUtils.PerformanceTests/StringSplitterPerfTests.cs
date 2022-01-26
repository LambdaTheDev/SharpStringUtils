using BenchmarkDotNet.Attributes;
using LambdaTheDev.SharpStringUtils.Iterator;

namespace SharpStringUtils.PerformanceTests
{
    [MemoryDiagnoser]
    public class StringSplitterPerfTests
    {
        private string _x = "dj02mud8.f29mf29e.ficw9m9m98.d892ud89qmwe.a0sdi9mq9wmduq.du09qm0duna.D9NUQDW9WUM.9NFUD9AMUSDW.uimf90me9wf.9w9fnwe9nf0wef.f0d9wnu9fnw0ew.0u9fn90ewufwe90funwemf.wfn9ewnfuew9f8nwc89dnsichs9iuhfc9a89rnuq89wufnd9ehc9sdhcv9nadsihciah9icmh9ashciahc0j0qj09uew90uf0wjcdioasmcsocjsidjv.smdjvidsjvcos.d9wqfud9mwefuwefnuc9dsnuf9ds0nfudsnf0";
        
        [GlobalSetup]
        public void Setup()
        {
            
        }
        
        [Benchmark]
        public void BenchmarkSplitter()
        {
            foreach (var str in new StringSplitterNonAlloc())
            {
                
            }
        }
        
        [Benchmark]
        public void BenchmarkSystem()
        {
            foreach (var str in _x.Split('.'))
            {
                
            }
        }
    }
}