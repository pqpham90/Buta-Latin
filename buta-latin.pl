#!/usr/bin/perl
use strict;
use warnings FATAL => 'all';
use constant IGNITE => 0;

while (<>) {
	chomp;

	if ($_ eq "exit") {
		last;
	}
	
	my @words_in_line = /[a-z0-9,!.?](?:[a-z0-9,!.?]*[a-z0-9,!.?])?/ig;
	my $result = "";

	foreach (@words_in_line) {
		# check if any letters exist
		if($_ =~ /\b[^a-z]+\b/i) {
			$result .= $_;
		}
		else {
			my $word = /\b([b-df-hj-np-tv-xz0-9]*)([\w]*)([,!.?]*)/ig;
			my $prefix = $1;
			my $stem = $2;
			my $ay = "ay";
			my $punctuation = $3;

			# print "word: $_\n";
			# print "prefix: $1\n";
			# print "stem: $2\n";
			# print "punctuation: $3\n";

			$prefix = lc($prefix);
			$stem = lc($stem);

			# keep first letter uppercase if needed
			if($_ =~ /^[[:upper:]]/) {
				$stem = ucfirst($stem);
			}

			if (IGNITE) {
				# check if all letters are vowels
				if($_ =~ /^[aeiouy]+\b/i) {
					$ay = "yay";
				}
			}
			else {
				# check if first letter is vowel
				if($_ =~ /^[aeiouy]/i) {
					$ay = "yay";
				}
			}

			$result .= "$stem$prefix" . "$ay$punctuation ";			
		}
	}

	chop($result);
	print "$result\n";
}