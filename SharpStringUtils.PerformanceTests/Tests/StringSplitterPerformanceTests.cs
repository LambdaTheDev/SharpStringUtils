using BenchmarkDotNet.Attributes;
using LambdaTheDev.SharpStringUtils.Iterator;

namespace LambdaTheDev.SharpStringUtils.PerformanceTests.Tests
{
    [MemoryDiagnoser]
    public class StringSplitterPerformanceTests
    {
        private StringSplitterNonAlloc _nonAlloc;

        private readonly string _str =
            "jsdjoisdmfidsf.f0dsjf9sdj.fji9asfn8euq8ew.hf9hds89q89msafa.ascj8nqe8nqfuca8sc.asdi0cmc9a8smd9asmdcas.jsdjoisdmfidsf.f0dsjf9sdj.fji9asfn8euq8ew.hf9hds89q89msafa.ascj8nqe8nqfuca8sc.asdi0cmc9a8smd9asmdcas.jsdjoisdmfidsf.f0dsjf9sdj.fji9asfn8euq8ew.hf9hds89q89msafa.ascj8nqe8nqfuca8sc.asdi0cmc9a8smd9asmdcas.jsdjoisdmfidsf.f0dsjf9sdj.fji9asfn8euq8ew.hf9hds89q89msafa.ascj8nqe8nqfuca8sc.asdi0cmc9a8smd9asmdcas.jsdjoisdmfidsf.f0dsjf9sdj.fji9asfn8euq8ew.hf9hds89q89msafa.ascj8nqe8nqfuca8sc.asdi0cmc9a8smd9asmdcas.jsdjoisdmfidsf.f0dsjf9sdj.fji9asfn8euq8ew.hf9hds89q89msafa.ascj8nqe8nqfuca8sc.asdi0cmc9a8smd9asmdcas.jsdjoisdmfidsf.f0dsjf9sdj.fji9asfn8euq8ew.hf9hds89q89msafa.ascj8nqe8nqfuca8sc.asdi0cmc9a8smd9asmdcas.jsdjoisdmfidsf.f0dsjf9sdj.fji9asfn8euq8ew.hf9hds89q89msafa.ascj8nqe8nqfuca8sc.asdi0cmc9a8smd9asmdcas.jsdjoisdmfidsf.f0dsjf9sdj.jsdjoisdmfidsf.f0dsjf9sdj.fji9asfn8euq8ew.hf9hds89q89msafa.ascj8nqe8nqfuca8sc.asdi0cmc9a8smd9asmdcas.jsdjoisdmfidsf.f0dsjf9sdj.fji9asfn8euq8ew.hf9hds89q89msafa.ascj8nqe8nqfuca8sc.asdi0cmc9a8smd9asmdcas.jsdjoisdmfidsf.f0dsjf9sdj.fji9asfn8euq8ew.hf9hds89q89msafa.ascj8nqe8nqfuca8sc.asdi0cmc9a8smd9asmdcas.jsdjoisdmfidsf.f0dsjf9sdj.fji9asfn8euq8ew.hf9hds89q89msafa.ascj8nqe8nqfuca8sc.asdi0cmc9a8smd9asmdcas.jsdjoisdmfidsf.f0dsjf9sdj.fji9asfn8euq8ew.hf9hds89q89msafa.ascj8nqe8nqfuca8sc.asdi0cmc9a8smd9asmdcas.jsdjoisdmfidsf.f0dsjf9sdj.fji9asfn8euq8ew.hf9hds89q89msafa.ascj8nqe8nqfuca8sc.asdi0cmc9a8smd9asmdcas.jsdjoisdmfidsf.f0dsjf9sdj.fji9asfn8euq8ew.hf9hds89q89msafa.ascj8nqe8nqfuca8sc.asdi0cmc9a8smd9asmdcas.jsdjoisdmfidsf.f0dsjf9sdj.fji9asfn8euq8ew.hf9hds89q89msafa.ascj8nqe8nqfuca8sc.asdi0cmc9a8smd9asmdcas.fji9asfn8euq8ew.hf9hds89q89msafa.ascj8nqe8nqfuca8sc.asdi0cmc9a8smd9asmdcas.";
        
        [GlobalSetup]
        public void Setup()
        {
            _nonAlloc = new StringSplitterNonAlloc(_str, '.');
        }

        [Benchmark]
        public void SplitStringNonAlloc()
        {
            foreach (StringSegment segment in _nonAlloc)
            {
                //
            }
        }

        [Benchmark]
        public void SplitStringSystem()
        {
            foreach (string str in _str.Split('.'))
            {
                
            }
        }
    }
}