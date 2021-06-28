using ArrayShiftDetection;
using LinkedListCycle;
using CycleTask;
using StringTask;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace TestProject
{
	public class StringTests
	{
		[Fact]
		public void StringTest()
		{
			Assert.Equal("ThisIsAnExample", StringReverse.ReverseWordsAndCamelCase("siht si na elpmaxe"));
			Assert.Equal("A", StringReverse.ReverseWordsAndCamelCase("a"));
			Assert.Equal("Word", StringReverse.ReverseWordsAndCamelCase("drow"));
			Assert.Equal("GitLab", StringReverse.ReverseWordsAndCamelCase("tig bal"));
			Assert.Equal("ABCDEF", StringReverse.ReverseWordsAndCamelCase("a b c d e f"));
			Assert.Equal("ReverseWordsAndCamelCase", StringReverse.ReverseWordsAndCamelCase("esrever sdrow dna lemac esac"));
			Assert.NotEqual("123", StringReverse.ReverseWordsAndCamelCase("123"));
			Assert.Equal("123", StringReverse.ReverseWordsAndCamelCase("321"));
			Assert.Equal("", StringReverse.ReverseWordsAndCamelCase(""));
		}
	}

	public class BinaryArrayToNumberTest
	{
		[Fact]
		public void FirstTest()
		{
			int[] test1 = new int[] { 0, 0, 0, 0 };
			int[] test2 = new int[] { 1, 1, 1, 1 };
			int[] test3 = new int[] { 0, 1, 1, 0 };
			int[] test4 = new int[] { 0, 1, 0, 1 };

			Assert.Equal(0, BinaryConvert.BinaryArrayToNumber(test1));
			Assert.Equal(15, BinaryConvert.BinaryArrayToNumber(test2));
			Assert.Equal(6, BinaryConvert.BinaryArrayToNumber(test3));
			Assert.Equal(5, BinaryConvert.BinaryArrayToNumber(test4));
		}
		[Fact]
		public void SecondTest()
		{
			for (int i = 0; i < 1000000; i++)
			{
				int[] iBinArr = Convert.ToString(i, 2).ToCharArray().Select(s => (int)Char.GetNumericValue(s)).ToArray();
				Assert.Equal(i, BinaryConvert.BinaryArrayToNumber(iBinArr));

			}
		}
	}

	public class NumericShiftDetectorTests
	{
		[Fact]
		public void TestSample1()
		{
			var sample = new[] { 15, 16, 18, 20, 1, 2, 5, 6, 7, 8, 11, 12 };
			var result = new NumericShiftDetector().GetShiftPosition(sample);
			Assert.Equal(expected: 4, actual: result);
		}

		[Fact]
		public void TestSample2()
		{
			var sample = new[] { 5, 6, 7, 8, 11, 12, 15, 16, 18, 20, 1, 2 };
			var result = new NumericShiftDetector().GetShiftPosition(sample);
			Assert.Equal(expected: 10, actual: result);
		}

		[Fact]
		public void TestSampleWithoutShift()
		{
			var sample = new[] { 1, 2, 5, 6, 7, 8, 11, 12, 15, 16, 18, 20 };
			var result = new NumericShiftDetector().GetShiftPosition(sample);
			Assert.Equal(expected: 0, actual: result);
		}

		[Fact]
		public void TestLargeSample()
		{
			var part1 = Enumerable.Range(15_000, 100_000); // 15000, 15001, ... 114998, 114999
			var part2 = Enumerable.Range(0, 14_995); // 0, 1, ... 14993, 14994
			var sample = part1.Concat(part2).ToArray(); // объединение двух последовательностей // 15000, 15001, ... 114998, 114999, 0, 1, ... 14993, 14994

			var result = new NumericShiftDetector().GetShiftPosition(sample);

			Assert.Equal(expected: 100_000, actual: result);
		}
	}

	public class LinkedListTest
	{
		[Fact]
		public void TestCycledList()
		{
			Assert.True(LinkedListCycleSearcher.FindCycle(new List<int> { 0 }));
			Assert.True(LinkedListCycleSearcher.FindCycle(new List<int> { 1, 1 }));
			Assert.True(LinkedListCycleSearcher.FindCycle(new List<int> { 1, 0 }));
			Assert.True(LinkedListCycleSearcher.FindCycle(new List<int> { 1, 2, 0 }));
			Assert.True(LinkedListCycleSearcher.FindCycle(new List<int> { 1, 2, 3, 4, 3, 6, 0 }));
			Assert.True(LinkedListCycleSearcher.FindCycle(new List<int> { 1, 2, 4, 5, 3, 2, 1 }));
			Assert.True(LinkedListCycleSearcher.FindCycle(new List<int> { 1, 2, 3, 4, 3, 6, 1 }));
		}
		[Fact]
		public void TestNonCycledList()
		{
			Assert.False(LinkedListCycleSearcher.FindCycle(new List<int> { 1 }));
			Assert.False(LinkedListCycleSearcher.FindCycle(new List<int> { 1, 2 }));
			Assert.False(LinkedListCycleSearcher.FindCycle(new List<int> { 1, 2, 3 }));
			Assert.False(LinkedListCycleSearcher.FindCycle(new List<int> { 1, 2, 30, 4, 5, 6, 7 }));
			Assert.False(LinkedListCycleSearcher.FindCycle(new List<int> { 1, 2, 4, 5, 3, 6, 7 }));
		}
	}
}
